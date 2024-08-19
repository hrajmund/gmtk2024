using Godot;
using System.Collections.Generic;
using Gmtk2024.Model;
using Gmtk2024.Scripts;
using Reference = Gmtk2024.Reference;

public class Hand : Node2D
{
	[Export]
	public PackedScene CardScene;
	private List<Card> _cards = new();
	private AudioStreamPlayer _audioPlayer;

	public List<Effect> CurrentPlayed { get; private set; }

	[Signal]
	public delegate void CardPlayed(bool hasMoreCards);

	public override void _Ready()
	{
		_audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
	}

	public override void _Process(float delta)
	{
		foreach (var card in _cards)
		{
			card.Playable = IsPlayableCard((int)card.Position.x, (int)card.Position.y);
			
		}
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
		int offset = (int)((GetViewportRect().Size.x) / 17);
		float startingPos = Position.x - (_cards.Count / 2) * offset;
		float startingRot = 0 - (_cards.Count / 2) * 5;

		int i = 0;
		foreach (var card in _cards)
		{
			card.RotationDegrees = startingRot;
			card.Setup(i, new Vector2(startingPos, Position.y + Mathf.Abs(startingRot * 2)), (int)card.RotationDegrees);
			i++;
			startingPos += offset;
			startingRot += 5;
		}
	}

	private void HandleCardPlay(int index, int xOn, int yOn)
	{
		if (!IsPlayableCard(xOn, yOn))
			return;
		List<Effect> effects = _cards[index].Effects;
		_cards[index].TriggerRemove();
		_cards.RemoveAt(index);
		RearrangeCards();
		// handle application here

		CurrentPlayed = effects;
		EmitSignal("CardPlayed", HasMoreCards());
		_audioPlayer.Play(.1f);
	}

	private bool IsPlayableCard(int xOn, int yOn)
	{
		return yOn < Position.y - 300;
	}

	public bool HasMoreCards()
	{
		return _cards.Count > 0;
	}
	
	public void ClearCards()
	{
		_cards.Clear();
		GetTree().CallGroup(Reference.CardGroup, "TriggerRemove");
	}
}
