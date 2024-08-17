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
	private Particles2D _particles2D;
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
		_particles2D = GetNode<Particles2D>("Particles2D");
		_sprite = GetNode<Sprite>("Sprite");
		AddToGroup(Reference.CardGroup);
	}

	public void Setup(int cardIndex, Vector2 position, int rotation)
	{
		_cardIndex = cardIndex;
		_startPos = position;
		_rotation = rotation;

		int textureUse = Effects.Count > 1 ? 7 : Effects[0].TextureIndex();

		StringBuilder mainBuilder = new StringBuilder();
		StringBuilder subBuilder = new StringBuilder();

		foreach (var effect in Effects)
		{
			mainBuilder.Append(effect.ShortHumanReadable());
			subBuilder.Append(effect.HumanReadable());
		}

		_sprite.Texture = _cardTextures[textureUse];

		_mainLabel.Text = mainBuilder.ToString();
		_subTextLabel.Text = subBuilder.ToString();
	}

	private void DisableDrag()
	{
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
		MouseExited();
		ZIndex = 0;
		_removing = true;
		_animationPlayer.Play("CardPlayed");
	}

	public override void _Process(float delta)
	{
		if (_dragging)
		{
			Vector2 mousePos = GetViewport().GetMousePosition();
			GlobalPosition = new Vector2(mousePos);
		}

		_particles2D.Emitting = Playable;
	}

	public override void _PhysicsProcess(float delta)
	{
		if (_removing || _dragging && _hovered)
			return;
		
		if(!Position.Equals(_startPos))
			Position = Position.LinearInterpolate(_startPos, .5f);
	}

	public override void _InputEvent(Object viewport, InputEvent @event, int shapeIdx)
	{
		GetTree().SetInputAsHandled();
		if (@event is InputEventMouseButton mouseEvent)
		{
			if(mouseEvent.ButtonIndex == 1 /*Button left*/ && @event.IsPressed())
				EnableDrag();
			else if(mouseEvent.ButtonIndex == 1 && !@event.IsPressed())
				DisableDrag();
		}
		else if (@event is InputEventScreenTouch screenTouchEvent)
		{
			if (screenTouchEvent.Pressed && screenTouchEvent.Index == 1)
			{
				Position = screenTouchEvent.Position;
				EnableDrag();
			}
			else if(!screenTouchEvent.Pressed && screenTouchEvent.Index == 1)
				DisableDrag();
		}
		else if (@event is InputEventMouseMotion mouseMotion)
		{
			MouseEntered();
		}
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
		
		if(_selected == this)
			_selected = null;
		ZIndex = 0;
		_hovered = false;
		RotationDegrees = _rotation;
		Scale = new Vector2(1, 1);
	}	
	
}
