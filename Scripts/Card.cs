using System;
using System.Collections.Generic;
using System.Text;
using Gmtk2024.Model;
using Godot;
using Object = Godot.Object;
using Vector2 = Godot.Vector2;

namespace Gmtk2024.Scripts;

public partial class Card : StaticBody2D
{
	private static Card _selected = null;

	[Export]
	private List<Texture> _cardTextures;

	private bool _dragging;
	private bool _removing;
	private bool _hovered;
	public bool Playable;

	private int _cardIndex;
	private int _rotation;
	private Vector2 _startPos;

	public List<Effect> Effects;

	private AnimationPlayer _animationPlayer;
	private CPUParticles2D _particles2D;
	private Sprite _sprite;

	[Signal]
	public delegate void DraggingStarted();

	[Signal]
	public delegate void DraggingStopped(int index, int xOn, int yOn);

	private Label _mainLabel;
	private Label _subTextLabel;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_mainLabel = GetNode<Label>("Sprite/MainLabel");
		_subTextLabel = GetNode<Label>("Sprite/SubTextLabel");
		_particles2D = GetNode<CPUParticles2D>("Particles2D");
		_sprite = GetNode<Sprite>("Sprite");
		AddToGroup(Reference.CardGroup);
	}

	public void Setup(int cardIndex, Vector2 position, int rotation)
	{
		_cardIndex = cardIndex;
		_startPos = position;
		_rotation = rotation;

		int textureUse = Effects.Count > 1 ? 7 : Effects[0].TextureIndex();

		switch (textureUse)
		{
			case 0:
				ChangeParticleColor(237, 28, 36);
				break;
			case 1: ChangeParticleColor(28, 189, 71);
				break;
			case 2: ChangeParticleColor(77, 110, 243);
				break;
			case 3: ChangeParticleColor(0, 255, 174);
				break;
			case 4: ChangeParticleColor(156, 66, 245);
				break;
			case 5: ChangeParticleColor(245, 103, 37);
				break;
			case 6: ChangeParticleColor(255, 238, 0);
				break;
			case 7: ChangeParticleColor(46, 46, 46);
				break;
		}

		StringBuilder mainBuilder = new StringBuilder();
		StringBuilder subBuilder = new StringBuilder();

		foreach (var effect in Effects)
		{
			mainBuilder.Append(effect.ShortHumanReadable() + ", ");
			subBuilder.Append(effect.HumanReadable() + "\n");
		}

		mainBuilder.Remove(mainBuilder.Length - 2, 2);
		subBuilder.Remove(subBuilder.Length - 1, 1);

		_sprite.Texture = _cardTextures[textureUse];

		_mainLabel.Text = mainBuilder.ToString();
		_subTextLabel.Text = subBuilder.ToString();
	}

	private void DisableDrag(bool forced)
	{
		if(!forced)
			EmitSignal("DraggingStopped", _cardIndex, Position.x, Position.y);
		_dragging = false;
		ZIndex -= 200;
	}

	private void EnableDrag()
	{
		if (_selected != this)
			return;
		_dragging = true;
		ZIndex += 200;
		EmitSignal("DraggingStarted");
	}

	public void TriggerRemove()
	{
		RemoveFromGroup(Reference.CardGroup);
		MouseExited();
		ZIndex = 0;
		_removing = true;
		_animationPlayer.Play("CardPlayed");
	}

	public override void _Process(float delta)
	{
		if (_dragging)
		{
			Vector2 mousePos = GetGlobalMousePosition();
			GlobalPosition = new Vector2(mousePos);
		}

		_particles2D.Emitting = Playable;
	}

	public override void _PhysicsProcess(float delta)
	{
		if (_removing || _dragging && _hovered)
			return;

		if (!Position.Equals(_startPos))
			Position = Position.LinearInterpolate(_startPos, .5f);
	}

	public override void _InputEvent(Object viewport, InputEvent @event, int shapeIdx)
	{
		GetTree().SetInputAsHandled();
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == 1 /*Button left*/ && @event.IsPressed())
				EnableDrag();
			else if (mouseEvent.ButtonIndex == 1 && !@event.IsPressed())
				DisableDrag(false);
			else if(mouseEvent.ButtonIndex == 2 && @event.IsPressed())
				DisableDrag(true);
		}
		else if (@event is InputEventScreenTouch screenTouchEvent)
		{
			if (screenTouchEvent.Pressed && screenTouchEvent.Index == 1)
			{
				Position = screenTouchEvent.Position;
				EnableDrag();
			}
			else if (!screenTouchEvent.Pressed && screenTouchEvent.Index == 1)
				DisableDrag(false);
		}
		else if (@event is InputEventMouseMotion mouseMotion)
		{
			MouseEntered();
		}
	}

	private void ChangeParticleColor(float r, float g, float b)
	{
		_particles2D.Color = new Color(r / 255.0f, g / 255.0f, b / 255.0f);
	}

	private void MouseEntered()
	{
		if (_removing || _selected != null)
			return;

		_selected = this;
		ZIndex = 200;
		_hovered = true;
		RotationDegrees = 0;
		Position += Vector2.Up * 50;
		Scale = new Vector2(1.5f, 1.5f);
	}

	private void MouseExited()
	{
		if (_removing && !_hovered)
			return;

		if (_selected == this)
			_selected = null;
		ZIndex = 0;
		_hovered = false;
		RotationDegrees = _rotation;
		Scale = new Vector2(1, 1);
	}

}
