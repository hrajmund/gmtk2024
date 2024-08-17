using System.Collections.Generic;
using Gmtk2024.Model;
using Godot;
using Vector2 = Godot.Vector2;

namespace Gmtk2024.Scripts;

public partial class Card : StaticBody2D
{
	private bool _dragging;
	private bool _removing;
	private bool _hovered;

	private int _cardIndex;
	private int _rotation;
	private Vector2 _startPos;

	private AnimationPlayer _animationPlayer;
	
	[Signal]
	public delegate void DraggingStarted();

	[Signal]
	public delegate void DraggingStopped(int index, int xOn, int yOn);

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		AddToGroup(Reference.CardGroup);
	}

	public void Setup(int cardIndex, Vector2 position, int rotation)
	{
		_cardIndex = cardIndex;
		_startPos = position;
		_rotation = rotation;
	}

	private void DisableDrag()
	{
		EmitSignal("DraggingStopped", _cardIndex, Position.x, Position.y);
		_dragging = false;
	}

	private void EnableDrag()
	{
		_dragging = true;
		
		EmitSignal("DraggingStarted");
	}

	public void TriggerRemove()
	{
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
	}  
	
	private void MouseEntered()
	{
		if (_removing)
			return;
		
		_hovered = true;
		RotationDegrees = 0;
		Position += Vector2.Up * 50;
		Scale = new Vector2(1.5f, 1.5f);
	}
	
	private void MouseExited()
	{
		if (_removing)
			return;
		
		_hovered = false;
		RotationDegrees = _rotation;
		Scale = new Vector2(1, 1);
	}	
	
}
