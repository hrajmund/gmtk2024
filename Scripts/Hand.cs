using Godot;
using System;
using System.Collections.Generic;
using Gmtk2024.Model;

public class Hand : Node2D
{
	public PackedScene Card;


	public void AddCards(List<List<Effect>> effects)
	{
		
	}

	public void ClearCards()
	{
		for (int i = 0; i < GetChildCount(); i++)
		{
			GetChild(i).QueueFree();
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
