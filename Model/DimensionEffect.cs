﻿using System;
using System.Text;
using Gmtk2024.Scripts;

namespace Gmtk2024.Model
{
    public class DimensionEffect : Effect
    {
        private Operation _operation;
        private Dimension _dimension;
        private float _value;
        
        public Operation Operation => _operation;
        public float Value => _value;
        public Dimension Dimension => _dimension;

        public DimensionEffect(Operation operation, Dimension dimension, float value)
        {
            _operation = operation;
            _dimension = dimension;
            _value = value;
        }

        public override void Apply(Coin coin)
        {
            
        }

        public override string HumanReadable()
        {
            StringBuilder readable = new StringBuilder();

            switch (Operation)
            {
                case Operation.Addition:
                    readable.Append("Adds to");
                    break;
                case Operation.Subtraction:
                    readable.Append("Takes from");
                    break;
                case Operation.Multiplication:
                    readable.Append("Multiplies");
                    break;
                case Operation.Divination:
                    readable.Append("Divides");
                    break;
            }

            readable.Append($" the Shape by {_value}x on the {_dimension.ToString()}");

            readable.Append(" Axis");

            return readable.ToString();
        }
    }
}