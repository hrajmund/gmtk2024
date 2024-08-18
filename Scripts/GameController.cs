using System;
using System.Collections.Generic;
using Gmtk2024.Model;
using Godot;

namespace Gmtk2024.Scripts;

public partial class GameController : Node
{
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
		_handManager = GetNode<Hand>("HandManager");
		_targetNugget = GetNode<GoldenNugget>("TargetNugget");
		_transformNugget = GetNode<GoldenNugget>("TransformNugget");
		_uiScript = GetNode<Control>("Camera2D/Control");
		
		Level = 1;
		Lives = 3;
	}

	private List<List<Effect>> GenerateProblem()
	{
		Random r = new Random();
		int randi = r.Next(3);
		
		ProblemGenerator generator = new ProblemGenerator((PolygonType)Enum.GetValues(typeof(PolygonType)).GetValue(randi), Level);

		var list = generator.GetProblems();

		return new ();
	}
	
	private void OnCardPlayed()
	{
		List<Effect> effects = _handManager.CurrentPlayed;

		foreach (var effect in effects)
		{
			effect.Apply(_targetNugget);
		}
	}
}



