using Godot;
using System;
using Gmtk2024.Model;
using System.Collections.Generic;


public class RajmundScene : Node2D
{
	public override void _Ready()
	{
		GDScript ProblemGeneratorScript = GD.Load<GDScript>("res://Scripts/Problem.gd");
		
		Godot.Object ProblemGeneratorNode = (Godot.Object)ProblemGeneratorScript.New();
		
		var transformations = (Godot.Collections.Array)ProblemGeneratorNode.Call("generate_problem", 5);
		
		
		foreach(var transformation in transformations) {
			Operation op = (Operation)transformation;
			float number = (float)ProblemGeneratorNode.Call("generate_problem_value", op, 1);
			GD.Print(op + " with: " + number);
		}
	}
}
