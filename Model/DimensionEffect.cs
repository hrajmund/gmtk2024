using System;
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

		public override void Apply(GoldenNugget nugget)
		{
			
		}

		public override string ShortHumanReadable()
		{
			return _value + "x";
		}

		public override string HumanReadable()
		{
			return Operation + " to the " + Dimension + " axis by " + _value + "x";
		}
	}
}
