using Godot;
using System;
using Gmtk2024.Model;
using System.Numerics;
using System.Threading;
using System.Collections.Generic;
using Vector2 = Godot.Vector2;

namespace Gmtk2024.Scripts
{
	public class GoldenNugget : Node2D
	{
		private Color color = new Color(1.0f, 0.843f, 0.0f); // Properly initialize color with normalized values
		private Color[] colors;
		public PolygonType polygonType;
		public List<float> PolygonData = new List<float>();
		private Texture texture;

		public override void _Ready()
		{
			colors = new Color[] { color };
			GD.Print(polygonType);
			texture = (Texture)GD.Load("res://PixelArts/gold.png");
		}

		public override void _Draw()
		{
			switch (polygonType)
			{
				case PolygonType.Circle:
					DrawCircle();
					break;
				case PolygonType.Triangle:
					DrawTriangle();
					break;
				case PolygonType.Square:
					DrawSquare();
					break;
				default:
					throw new InvalidOperationException("Unknown polygon type.");
			}
		}

		private void DrawCircle()
		{
			const int nbPoints = 360;
			var pointsArc = new Vector2[nbPoints];
			var uvsArc = new Vector2[nbPoints];
			Vector2 center = new Vector2(0, 0);
			float rotationRad = Mathf.Deg2Rad(360.0f - PolygonData[2]);

			for (int i = 0; i < nbPoints; i++)
			{
				float anglePoint = Mathf.Deg2Rad(i * 360.0f / nbPoints - 90);
				float x = Mathf.Cos(anglePoint) * PolygonData[0];
				float y = Mathf.Sin(anglePoint) * PolygonData[1];

				float rotatedX = x * Mathf.Cos(rotationRad) - y * Mathf.Sin(rotationRad);
				float rotatedY = x * Mathf.Sin(rotationRad) + y * Mathf.Cos(rotationRad);

				pointsArc[i] = center + new Vector2(rotatedX, rotatedY);
				uvsArc[i] = new Vector2(0.5f + rotatedX / (2 * PolygonData[0]), 0.5f + rotatedY / (2 * PolygonData[1]));
			}

			DrawPolygon(pointsArc, null, uvsArc, texture);
		}

		private void DrawTriangle()
		{
			var trianglePoints = new Vector2[3];
			trianglePoints[0] = new Vector2(PolygonData[0], PolygonData[1]);
			trianglePoints[1] = new Vector2(PolygonData[2], PolygonData[3]);
			trianglePoints[2] = new Vector2(PolygonData[4], PolygonData[5]);

			var uvsTriangle = new Vector2[3]
			{
				new Vector2(0.5f, 0.0f),
				new Vector2(1.0f, 1.0f),
				new Vector2(0.0f, 1.0f)
			};

			DrawPolygon(trianglePoints, null, uvsTriangle, texture);
		}

		private void DrawSquare()
		{
			var squarePoints = new Vector2[4]
			{
				new Vector2(PolygonData[0], PolygonData[1]),
				new Vector2(PolygonData[2], PolygonData[3]),
				new Vector2(PolygonData[4], PolygonData[5]),
				new Vector2(PolygonData[6], PolygonData[7])
			};

			var uvsSquare = new Vector2[4]
			{
				new Vector2(0.0f, 0.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(1.0f, 1.0f),
				new Vector2(0.0f, 1.0f)
			};

			DrawPolygon(squarePoints, null, uvsSquare, texture);
		}
		public void setPolygonType(PolygonType _polygonType, int rotateType = 1, int radiusA = 0, int radiusB = 0)
		{
			polygonType = _polygonType;
			PolygonData.Clear();

			if (polygonType == PolygonType.Triangle)
			{
				PolygonData.Add(-100); PolygonData.Add(100);  // A
				PolygonData.Add(100); PolygonData.Add(100);   // B
				PolygonData.Add(0); PolygonData.Add(-100);    // C
			}
			else if (polygonType == PolygonType.Square)
			{
				PolygonData.AddRange(new float[] { -100, -100, 100, -100, 100, 100, -100, 100 }); // A, B, C, D
			}
			else if (polygonType == PolygonType.Circle)
			{
				PolygonData.Add(radiusA);
				PolygonData.Add(radiusB);
				PolygonData.Add((float)rotateType);
			}
		}
		public override void _Process(float delta)
		{
			Update(); // Continuously redraw to reflect changes
		}
		public void updateCircleAddition(float value, Dimension dimension)
		{
			if (PolygonData[0] == PolygonData[1])
			{
				PolygonData[0] += PolygonData[0] * value;
				PolygonData[1] += PolygonData[1] * value;
			}
			else
			{
				if (dimension == Dimension.X)
				{
					PolygonData[0] += PolygonData[0] * value;
				}
				else
				{
					PolygonData[1] += PolygonData[1] * value;
				}
			}
		}
		public void updateCircleDivison(float value, Dimension dimension)
		{
			if (PolygonData[0] == PolygonData[1])
			{
				PolygonData[0] /= value;
				PolygonData[1] /= value;
			}
			else
			{
				if (dimension == Dimension.X)
				{
					PolygonData[0] /= value;
				}
				else
				{
					PolygonData[1] /= value;
				}
			}
		}
		public void updateCircleMultiplication(float value, Dimension dimension)
		{
			if (PolygonData[0] == PolygonData[1])
			{
				PolygonData[0] *= value;
				PolygonData[1] *= value;
			}
			else
			{
				if (dimension == Dimension.X)
				{
					PolygonData[0] *= value;
				}
				else
				{
					PolygonData[1] *= value;
				}
			}
		}
		public void updateCircleSubstraction(float value, Dimension dimension)
		{
			if (PolygonData[0] == PolygonData[1])
			{
				PolygonData[0] -= PolygonData[0] * value;
				PolygonData[1] -= PolygonData[1] * value;
			}
			else
			{
				if (dimension == Dimension.X)
				{
					PolygonData[0] -= PolygonData[0] * value;
				}
				else
				{
					PolygonData[1] -= PolygonData[1] * value;
				}
			}
		}
		public void updateCirclePower(float value, Dimension dimension)
		{
			if (PolygonData[0] == PolygonData[1])
			{
				PolygonData[0] = Mathf.Pow(PolygonData[0], value);
				PolygonData[1] = Mathf.Pow(PolygonData[1], value);
			}
			else
			{
				if (dimension == Dimension.X)
				{
					PolygonData[0] = Mathf.Pow(PolygonData[0], value);
				}
				else
				{
					PolygonData[1] = Mathf.Pow(PolygonData[1], value);
				}
			}
		}
		public void updateCircleRoot(float value, Dimension dimension)
		{
			if (PolygonData[0] == PolygonData[1])
			{
				PolygonData[0] = Mathf.Pow(PolygonData[0], 1 / value);
				PolygonData[1] = Mathf.Pow(PolygonData[1], 1 / value);
			}
			else
			{
				if (dimension == Dimension.X)
				{
					PolygonData[0] = Mathf.Pow(PolygonData[0], 1 / value);
				}
				else
				{
					PolygonData[1] = Mathf.Pow(PolygonData[1], 1 / value);
				}
			}
		}
		public void updateAddition(float value, Dimension dimension)
		{
			for (int i = 0; i < PolygonData.Count; i++)
			{
				PolygonData[i] += PolygonData[i] * value;
			}
		}
		public void updateDivison(float value, Dimension dimension)
		{
			for (int i = 0; i < PolygonData.Count; i++)
			{
				PolygonData[i] /= value;
			}
		}
		public void updateMultiplication(float value, Dimension dimension)
		{
			for (int i = 0; i < PolygonData.Count; i++)
			{
				PolygonData[i] *= value;
			}
		}
		public void updateSubstraction(float value, Dimension dimension)
		{
			for (int i = 0; i < PolygonData.Count; i++)
			{
				PolygonData[i] -= PolygonData[i] * value;
			}
		}
		public void updatePower(float value, Dimension dimension)
		{
			for (int i = 0; i < PolygonData.Count; i++)
			{
				PolygonData[i] = Mathf.Pow(PolygonData[i], value);
			}
		}
		public void updateRoot(float value, Dimension dimension)
		{
			for (int i = 0; i < PolygonData.Count; i++)
			{
				PolygonData[i] = Mathf.Pow(PolygonData[i], 1 / value);
			}
		}
		public void updatePolygon(Operation operation, float value, Dimension dimension)
		{
			switch (operation)
			{
				case Operation.Addition:
					{
						if (polygonType == PolygonType.Circle)
						{
							updateCircleAddition(value, dimension);
						}
						else
						{
							updateAddition(value, dimension);
						}
						break;
					}
				case Operation.Division:
					{
						if (polygonType == PolygonType.Circle)
						{
							updateCircleDivison(value, dimension);
						}
						else
						{
							updateDivison(value, dimension);
						}
						break;
					}
				case Operation.Multiplication:
					{
						if (polygonType == PolygonType.Circle)
						{
							updateCircleMultiplication(value, dimension);
						}
						else
						{
							updateMultiplication(value, dimension);
						}
						break;
					}
				case Operation.Subtraction:
					{
						if (polygonType == PolygonType.Circle)
						{
							updateCircleSubstraction(value, dimension);
						}
						else
						{
							updateSubstraction(value, dimension);
						}
						break;
					}
				case Operation.Power:
					{
						if (polygonType == PolygonType.Circle)
						{
							updateCirclePower(value, dimension);
						}
						else
						{
							updatePower(value, dimension);
						}
						break;
					}
				case Operation.Rotation:
					this.RotationDegrees += value;
					break;
			}
			Update();
		}
		/*
		public void setPolygonType(PolygonType _polygonType, int rotateType = 1, int radiusA = 0, int radiusB = 0)
		{
			polygonType = _polygonType;
			PolygonData.Clear();
			PolygonData.Clear();
			GD.Print(polygonType);
			if (polygonType == PolygonType.Triangle)
			{
				PolygonData.Add(-100); PolygonData.Add(100);      //A
				PolygonData.Add(100); PolygonData.Add(100);       //B
				PolygonData.Add(0); PolygonData.Add(-100);        //C
			}
			else if (polygonType == PolygonType.Square)
			{
				PolygonData.AddRange(new float[] { -100, -100, 100, -100, 100, 100, -100, 100 });  // A, B, C, D
			}
			else
			{
				PolygonData.Add(radiusA);
				PolygonData.Add(radiusB);
				PolygonData.Add((float)rotateType);
			}
		}*/
		// TODO: add treshold based on level
		public float Treshold(int levelNum){
			if(levelNum == 1)
				return 0.1f;
			else if(levelNum < 5)
				return 0.2f;
			else if(levelNum < 9)
				return 0.3f;
			else if(levelNum < 14)
				return 0.4f;
			else if(levelNum < 24)
				return 0.5f;
			else
				return 0.6f;
		}
		public bool Compare(GoldenNugget other, int levelNum)
		{
			if (this.polygonType != other.polygonType)
				return false;

			float diff = 0;
			int sides = 0;
			float maxAllowedDiff = Treshold(levelNum); 

			switch (polygonType)
			{
				case PolygonType.Circle:
					sides = PolygonData.Count;

					List<float> normalizedThisPolygonData = ShapeNormalization.NormalizeCircleData(this.PolygonData);
					List<float> normalizedOtherPolygonData = ShapeNormalization.NormalizeCircleData(other.PolygonData);

					for (int i = 0; i < sides; i++)
					{
						diff += Mathf.Abs(normalizedThisPolygonData[i] - normalizedOtherPolygonData[i]);
					}

					GD.Print("NormalizedTransformCircle: " + String.Join(";", normalizedThisPolygonData));
					GD.Print("NormalizedTargetCircle: " + String.Join(";", normalizedOtherPolygonData));
					break;
				case PolygonType.Triangle:
					sides = 3;
					var normalizedThisTriangle = ShapeNormalization.NormalizeTriangle(this.PolygonData);
					var normalizedOtherTriangle = ShapeNormalization.NormalizeTriangle(other.PolygonData);
					
					for (int i = 0; i < normalizedThisTriangle.Count; i++) {
						diff += Mathf.Abs(normalizedThisTriangle[i] - normalizedOtherTriangle[i]);
					}

					GD.Print("NormalizedTransformTriangle " + String.Join(";", normalizedThisTriangle));
					GD.Print("NormalizedTargetTriangle " + String.Join(";", normalizedOtherTriangle));
					break;
				case PolygonType.Square:
					sides = 4;
					var normalizedThisSquare = ShapeNormalization.NormalizeSquare(this.PolygonData);
					var normalizedOtherSquare = ShapeNormalization.NormalizeSquare(other.PolygonData);
					
					for (int i = 0; i < normalizedThisSquare.Count; i++) {
						diff += Mathf.Abs(normalizedThisSquare[i] - normalizedOtherSquare[i]);
					}
					GD.Print("NormalizedTransformSquare " + String.Join(";", normalizedThisSquare));
					GD.Print("NormalizedTargetSquare " + String.Join(";", normalizedOtherSquare));
					break;
			}

			float relativeDiff = Mathf.Sqrt(diff / sides);
			GD.Print("Relative diff: " + relativeDiff);

			return relativeDiff < maxAllowedDiff;
		}
	}
}
