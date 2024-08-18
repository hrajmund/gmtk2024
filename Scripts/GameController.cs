using System;
using System.Collections.Generic;
using Gmtk2024.Model;
using Godot;

namespace Gmtk2024.Scripts;

public partial class GameController : Node
{
	[Export] private int LevelMergeFrom = 5;
	
	private int _lives = 3;
	private int _level = 1;

	public int Lives
	{
		get => _lives;
		set
		{
			_lives = value;
			_uiScript.Set("health", value);

		}
	}

	public int Level
	{
		get => _level;
		set
		{
			_level = value;
			_uiScript.Set("level", value);
		}
	}

	private Hand _handManager;

	private GoldenNugget _targetNugget;
	private GoldenNugget _transformNugget;

	private Control _uiScript;

	public override void _Ready()
	{
		GD.Randomize();
		_handManager = GetNode<Hand>("HandManager");
		_targetNugget = GetNode<GoldenNugget>("TargetNugget");
		_transformNugget = GetNode<GoldenNugget>("TransformNugget");
		_uiScript = GetNode<Control>("Camera2D/Control");

		Level = 1;
		Lives = 3;
		
		BeginLevel();
	}

	private void BeginLevel()
	{
		uint randi = GD.Randi() % 3;

		var polygonType = (PolygonType)Enum.GetValues(typeof(PolygonType)).GetValue(randi);

		uint rotateType = 0;

		switch (polygonType)
		{
			case PolygonType.Triangle:
				rotateType = GD.Randi() % 8 + 1;
				break;
			case PolygonType.Square:
				rotateType = GD.Randi() % 2 + 1;
				break;
			case PolygonType.Circle:
				rotateType = (GD.Randi() % 24 + 1) * 15;
				break;
		}
		
		_targetNugget.setPolygonType(polygonType, (int)rotateType, (int)(GD.Randi() % 200 + 100), (int)(GD.Randi() % 200 + 100));
		_transformNugget.setPolygonType(polygonType, (int)rotateType, (int)(GD.Randi() % 200 + 100), (int)(GD.Randi() % 200 + 100));
		
		var list = GenerateProblem(polygonType);

		_handManager.SetCards(list);

		foreach (var effects in list)
		{
			ApplyEffectTo(_targetNugget, effects);
		}
	}

	private List<List<Effect>> GenerateProblem(PolygonType type)
	{
		ProblemGenerator generator = new ProblemGenerator(type, Level);

		var list = generator.GetProblems();

		int maxGen = 10 - list.Count;

		int dummyCardGenNumber = (int)(GD.Randi() % (maxGen + 1));

		while (dummyCardGenNumber > 0)
		{
			Effect card = (Effect)list[(int)(GD.Randi() % list.Count)].Clone();

			card.Randomize(_level);
			
			list.Add(card);

			dummyCardGenNumber--;
		}

		List<List<Effect>> result = new();

		foreach (var effect in list)
		{
			result.Add(new List<Effect>() {effect});
		}

		if (_level >= LevelMergeFrom)
		{
			uint mergeCards = GD.Randi() % (uint)((result.Count / 5.0f) + 0.5f);

			while (mergeCards > 0)
			{
				int oneCard = (int)(GD.Randi() % result.Count);

				int secondCard = (int)(GD.Randi() % result.Count);

				while (secondCard == oneCard)
				{
					secondCard = (int)(GD.Randi() % result.Count);
				}

				List<Effect> oneList = result[oneCard];
				
				result[secondCard].AddRange(oneList);
				
				result.RemoveAt(oneCard);

				mergeCards--;
			}
		}

		int modBy = (result.Count - 3) + 2;
		if (modBy <= 0)
			modBy = 2;
		uint len = (uint)(GD.Randi() % modBy);

		for (int i = 0; i < len; i++)
		{
			ApplyEffectTo(_targetNugget, result[i]);
		}

		return result;
	}

	private void OnTurnFinnish()
	{
		var result = _transformNugget.Compare(_targetNugget);

		if (result)
			Level++;
		else Lives--;

		if (Lives > 0)
			BeginLevel();
		else // game over
			return;
	}

	private void ApplyEffectTo(GoldenNugget nugget, List<Effect> effects)
	{
		foreach (var effect in effects)
		{
			effect.Apply(nugget);
		}
	}

	private void OnCardPlayed()
	{
		ApplyEffectTo(_transformNugget, _handManager.CurrentPlayed);        
		
		GD.Print("Are equal: ", _transformNugget.Compare(_targetNugget));
	}
}



