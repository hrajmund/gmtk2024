using System;
using System.Collections.Generic;
using Gmtk2024.Model;
using Godot;

namespace Gmtk2024.Scripts;

public partial class GameController : Node
{
	[Export] private int LevelMergeFrom = 5;
	
	[Export] private int _lives = 3;
	[Export] private int _level = 1;
	

	public int Lives
	{
		get => _lives;
		set
		{
			_lives = value;
			_uiScript.Call("_set_health", value);
			UpdateGoblinState();
		}
	}

	public int Level
	{
		get => _level;
		set
		{
			_level = value;
			_uiScript.Call("_set_level", value);
		}
	}

	private Hand _handManager;

	[Export]
	private PackedScene _nuggetScene;

	private GoldenNugget _targetNugget;
	private GoldenNugget _transformNugget;

	private Control _uiScript;
	private AnimationPlayer _animationPlayer;
	private AnimatedSprite _wizard;
	private AnimatedSprite _goblin;
	private MessageBubbleLabel _goblinLabel;

	public override void _Ready()
	{
		GD.Randomize();
		_handManager = GetNode<Hand>("HandManager");
		_uiScript = GetNode<Control>("Camera2D/Control");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_wizard = GetNode<AnimatedSprite>("Wizard");
		_goblinLabel = GetNode<MessageBubbleLabel>("Goblin/Sprite/Label");
		_goblin = GetNode<AnimatedSprite>("Goblin");
		
		Level = _level;
		Lives = _lives;
		_animationPlayer.Play("GameBegin");
	}

	private void ClearNuggets() {
		Node2D node = GetNode<Node2D>("Node2D");

		if (_targetNugget != null) {
			GD.Print("_targetNugget cleaned");
			_targetNugget.QueueFree();
		}
		_targetNugget = _nuggetScene.Instance() as GoldenNugget;
		_targetNugget.Position = new Vector2(1282, 330);
		node.AddChild(_targetNugget);

		if (_transformNugget != null) {
			GD.Print("_transformNugget cleaned");
			_transformNugget.QueueFree();
		}
		_transformNugget = _nuggetScene.Instance() as GoldenNugget;
		_transformNugget.Position = new Vector2(625, 330);
		node.AddChild(_transformNugget);
	}

	private void BeginLevel()
	{
		uint randi = GD.Randi() % 3;

		var polygonType = (PolygonType)Enum.GetValues(typeof(PolygonType)).GetValue(randi);

		uint rotateType = 0;

		if (polygonType == PolygonType.Circle)
			rotateType = (GD.Randi() % 24 + 1) * 15;

//		Vector2 dimensions = new Vector2(GD.Randi() % 200 + 100, GD.Randi() % 200 + 100);
		Vector2 dimensions = new Vector2((GD.Randi() % 50) + 20, (GD.Randi() % 50) + 20); 
		//Vector2 balancedDimension = new Vector2(dimensions.x > 50 ? dimensions.x/2 : dimensions.x,
		//	dimensions.y > 50 ? dimensions.y/2: dimensions.y);
		ClearNuggets();
		
//		_targetNugget.setPolygonType(polygonType, (int)rotateType, (int)balancedDimension.x, (int)balancedDimension.y);
//		_transformNugget.setPolygonType(polygonType, (int)rotateType, (int)balancedDimension.x, (int)balancedDimension.y);

		_targetNugget.setPolygonType(polygonType, (int)rotateType, (int)dimensions.x, (int)dimensions.y);
		_transformNugget.setPolygonType(polygonType, (int)rotateType, (int)dimensions.x, (int)dimensions.y);

		
		var list = GenerateProblem(polygonType);

		_handManager.SetCards(list);
		
		if (polygonType == PolygonType.Circle)
		{
			GD.Print("Circle data");
			GD.Print(_transformNugget.PolygonData[0]);
			GD.Print(_transformNugget.PolygonData[1]);
			GD.Print(_transformNugget.PolygonData[2]);
			
		}
	}

	private List<List<Effect>> GenerateProblem(PolygonType type)
	{
		ProblemGenerator generator = new ProblemGenerator(type, Level);

		var list = generator.GetProblems();

		List<List<Effect>> result = new();

		foreach (var effect in list)
		{
			result.Add(new List<Effect>() {effect});
		}

		if (_level >= LevelMergeFrom)
		{
			uint mergeCards = (uint)(GD.Randi() % result.Count);
			GD.Print(mergeCards + " Cards can be merged");
			while (mergeCards > 0)
			{
				if (GD.Randf() >= .4f)
				{
					mergeCards--;
					continue;
				}
				int oneCard = (int)(GD.Randi() % result.Count);

				if (result[oneCard].Count >= 3)
				{
					mergeCards--;
					continue;
				}

				int secondCard = (int)(GD.Randi() % result.Count);

				while (secondCard == oneCard)
				{
					secondCard = (int)(GD.Randi() % result.Count);
				}
				
				if (result[secondCard].Count >= 3)
				{
					mergeCards--;
					continue;
				}

				List<Effect> oneList = result[oneCard];
				
				result[secondCard].AddRange(oneList);
				
				result.RemoveAt(oneCard);

				mergeCards--;
			}
		}
		
		bool[] flags = new bool[result.Count];

		int iter = Mathf.RoundToInt((float)result.Count / 2 + .5f);
		for (int i = 0; i < iter; i++)
		{
			int rand = (int)(GD.Randi() % result.Count);
			while(flags[rand])
				rand = (int)(GD.Randi() % result.Count);
			ApplyEffectTo(_targetNugget, result[rand]);
			flags[rand] = true;
		}

		return result;
	}

	private void UpdateGoblinState()
	{
		switch (_lives)
		{
			case 3: _goblin.Animation = "default";
				break;
			case 2: _goblin.Animation = "SecondPhase";
				break;
			case 1: _goblin.Animation = "ThirdPhase";
				break;
		}
	}

	private void OnTurnFinnish()
	{
		var result = _transformNugget.Compare(_targetNugget, Level);
		_handManager.ClearCards();

		if (result) 
		{
			_goblinLabel.SetRandomPositive();
			Level++;
		}
		else
		{
			_goblinLabel.SetRandomNegative();
			Lives--;
		}

		if (Lives > 0)
			_animationPlayer.Play("GoblinSubmitGold");
		else GameOver();
			return;
	}

	private void GameOver()
	{
		_animationPlayer.Play("GameEnd");
		
		var musicScript = GetNode("/root/Music");
		musicScript.Call("stopMusic");
	}

	private void LoadMainScene()
	{
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}

	private void ApplyEffectTo(GoldenNugget nugget, List<Effect> effects)
	{
		GD.Print("NEW");
		foreach (var effect in effects)
		{
			GD.Print("===================================");
			GD.Print(effect.HumanReadable());
			effect.Apply(nugget);
		}
	}

	private void OnCardPlayed(bool hasMoreCards)
	{
		ApplyEffectTo(_transformNugget, _handManager.CurrentPlayed);      
		
		GD.Print("Are equal: ", _transformNugget.Compare(_targetNugget, Level));
		
		
		if (!hasMoreCards)
		{
			OnTurnFinnish();
		}
		else _animationPlayer.Play("CardPlayed");

	}
}



