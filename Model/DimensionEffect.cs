using System;
using Gmtk2024.Scripts;
using Godot;

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

        public override void Apply(GoldenNugget nugget)
        {
            nugget.updatePolygon(_operation, _value, _dimension);
        }

        public override string ShortHumanReadable()
        {
            return _value + "x";
        }

        public override string HumanReadable()
        {
            return Operation + " to the " + Dimension + " axis by " + _value + "x";
        }

        public override int TextureIndex()
        {
            switch (_operation)
            {
                case Operation.Addition:
                    return 0;
                case Operation.Subtraction:
                    return 6;
                case Operation.Multiplication:
                    return 2;
                case Operation.Division:
                    return 1;
                case Operation.Rotation:
                    return 5;
                case Operation.Power:
                    return 3;
                case Operation.Root:
                    return 4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_operation), _operation, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns>True if randomize was successfull happened</returns>
        public override bool Randomize(int level)
        {
            if (Operation == Operation.Rotation)
                return false;

            float diff = GD.Randf() * (1 - level * 0.01f) * (GD.Randi() % 2 == 0 ? -1 : 1);

            int wholePart = (int)(_value + diff);
            float fraction = Mathf.RoundToInt((_value + diff - wholePart) * 10);
            _value = fraction / 10 + wholePart;

            return true;
        }

        public override DimensionEffect Clone()
        {
            return new DimensionEffect(Operation, Dimension, _value);
        }
    }
}
