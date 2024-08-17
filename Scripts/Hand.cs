using Godot;
using System;
using System.Collections.Generic;
using Gmtk2024.Model;
using Gmtk2024.Scripts;
using Reference = Gmtk2024.Reference;

public class Hand : Node2D
{
	[Export]
	public PackedScene CardScene;

	private List<List<Effect>> _effects;

	public override void _Ready()
	{
		List<List<Effect>> eff = new List<List<Effect>>();

		for (int i = 0; i < 10; i++)
		{
			eff.Add(new List<Effect>());
		}

		SetCards(eff);
	}

	public void SetCards(List<List<Effect>> effects)
	{
		ClearCards();
		_effects = effects;

		int offset = 150;
		float startingPos = 0 - (effects.Count / 2) * offset;

		int i = 0;
		foreach (var effect in _effects)
		{
			Card inst = CardScene.Instance() as Card;
			AddChild(inst);
			
			inst.Position = Position;
			inst.LookAt(new Vector2(GlobalPosition.x + 300, GlobalPosition.y - 200));
			inst.Setup(i, new Vector2(startingPos, Position.y), (int)inst.RotationDegrees);

			i++;
			startingPos += offset;
		}
	}

	public void ClearCards()
	{
		GetTree().CallGroup(Reference.CardGroup, "TriggerRemove");
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
