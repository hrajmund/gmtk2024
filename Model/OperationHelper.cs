using Godot;
using System;

namespace Gmtk2024.Model {
	public class OperationHelper : Node
	{
		public Operation GetAddition() { return Operation.Addition; }
		public Operation GetSubtraction() { return Operation.Subtraction; }
		public Operation GetMultiplication() { return Operation.Multiplication; }
		public Operation GetDivination() { return Operation.Divination; }
	}
}
