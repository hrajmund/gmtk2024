using Godot;
using System;
using System.Collections.Generic;

namespace Gmtk2024.Model
{

    public class ProblemGenerator
    {
        private PolygonType _polygonType;
        private int _level;
        private Godot.Object _problemGeneratorNode;

        public ProblemGenerator(PolygonType polygonType, int level)
        {
            _polygonType = polygonType;
            _level = level;

            GDScript ProblemGeneratorScript = GD.Load<GDScript>("res://Scripts/Problem.gd");
            _problemGeneratorNode = (Godot.Object)ProblemGeneratorScript.New();
        }

        public List<Effect> GetProblems()
        {
            List<Effect> problems = new();
            var transformations = (Godot.Collections.Array)_problemGeneratorNode.Call("generate_problem", _level);
            Random r = new Random();
            foreach (var transformation in transformations)
            {
                
                Operation op = (Operation)transformation;
                float number = (float)_problemGeneratorNode.Call("generate_problem_value", op, _level);
                problems.Add(new DimensionEffect(op, r.Next(2) == 0 ? Dimension.X : Dimension.Y, number));
            }

            return problems;
        }

    }

}
