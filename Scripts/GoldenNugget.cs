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
		public PolygonType polygonType;
		public List<float> PolygonData = new List<float>();
		private Texture texture;
		private bool isInitialized = false;
		public override void _Ready()
		{
			texture = (Texture)GD.Load("res://PixelArts/gold.png");
		}

		public override void _Draw()
		{
			if (!isInitialized)
				return;
			
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
			isInitialized = true;
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
		public void updatePolygon(Operation operation, float value, Dimension dimension)
		{
			GD.Print(this.GlobalTransform.Scale);
			switch (operation)
			{
				case Operation.Addition:
				{
					if (dimension == Dimension.X)
						this.Scale += Scale * new Vector2(value, 0);
					else
						this.Scale += Scale * new Vector2(0, value);
					break;
					}
				case Operation.Division:
					{
						if (dimension == Dimension.X)
							this.Scale /= new Vector2(value, 1);
						else
							this.Scale /= new Vector2(1, value);
						break;
					}
				case Operation.Multiplication:
					{
						if (dimension == Dimension.X)
							this.Scale *= new Vector2(value, 1);
						else
							this.Scale *= new Vector2(1, value);
						break;
					}
				case Operation.Subtraction:
					{
						if (dimension == Dimension.X)
							this.Scale -= Scale * new Vector2(value, 0);
						else
							this.Scale -= Scale * new Vector2(0, value);
						break;
					}
				case Operation.Rotation:
					this.RotationDegrees += value;
					break;
			}
			Update();
		}
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

			float diff = Mathf.Sqrt(Mathf.Pow(this.Scale.x - other.Scale.x,2) + Mathf.Pow(this.Scale.y - other.Scale.y, 2));
			diff += Mathf.Abs(this.RotationDegrees - other.RotationDegrees) % 180;

			GD.Print("DIFF:" + diff);

			if (diff < Treshold(levelNum))
				return true;

			return false;
		}
	}
}
