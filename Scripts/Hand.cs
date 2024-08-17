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
	private List<Card> _cards = new ();

	public override void _Ready()
	{
		List<List<Effect>> eff = new List<List<Effect>>();

		for (int i = 0; i < 10; i++)
		{
			eff.Add(new List<Effect> {new DimensionEffect(Operation.Addition, Dimension.X, 10)});
		}

		SetCards(eff);
	}

	public void SetCards(List<List<Effect>> effects)
	{
		ClearCards();
		foreach (var effect in effects)
		{
			Card inst = CardScene.Instance() as Card;
			AddChild(inst);

			inst.Position = Position;
			inst.Effects = effect;
			_cards.Add(inst);
			inst.Connect("DraggingStopped", this, "HandleCardPlay");
		}
		
		RearrangeCards();
	}

	private void RearrangeCards()
	{
		int offset = (int)((GetViewportRect().Size.x) / 15);
		float startingPos = Position.x - (_cards.Count / 2) * offset;
		float startingRot = 0 - (_cards.Count / 2) * 5;

		int i = 0;
		foreach (var card in _cards)
		{
			card.RotationDegrees = startingRot;
			card.Setup(i, new Vector2(startingPos, Position.y), (int)card.RotationDegrees);
			i++;
			startingPos += offset;
			startingRot += 5;
		}
	}

	private void HandleCardPlay(int index, int xOn, int yOn)
	{
		if (yOn > Position.y - 200)
			return;
		_cards[index].TriggerRemove();
		_cards.RemoveAt(index);
		RearrangeCards();
		// handle application here
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
