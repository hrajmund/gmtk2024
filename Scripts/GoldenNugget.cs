using Godot;
using System;
using Gmtk2024.Model;
using System.Numerics;
using System.Threading;
using Vector2 = Godot.Vector2;

namespace Gmtk2024.Scripts{
	public class GoldenNugget : Node2D
	{
		private Color color = new Color(255,215,0);
		private Color[] colors;
		private bool drawSquare = false;
		public PolygonType polygonType;
		public void setPolygonType(PolygonType _polygonType){
			polygonType = _polygonType;
		}
		public override void _Ready()
		{
			colors = new Color[] { color };
			setPolygonType(PolygonType.Square);
			// Set up and start the Timer
			Godot.Timer timer = new Godot.Timer();
			timer.WaitTime = 5.0f; // 5 seconds
			timer.OneShot = true;  // The timer will only trigger once
			timer.Connect("timeout", this, nameof(OnTimerTimeout));
			AddChild(timer);
			timer.Start();

		}

		public override void _Draw()
		{
			switch(polygonType){
				case PolygonType.Circle :
					//Draw Circle
					int nbPoints = 360;
					var pointsArc = new Godot.Vector2[nbPoints];
					Godot.Vector2 center = new Godot.Vector2(20,20);
					float radius = 150.0f;
					float angleTo = 360.0f;
					float angleFrom = 0.0f;
					for (int i = 0; i < nbPoints; ++i)
					{
						float anglePoint = Mathf.Deg2Rad(angleFrom + i * (angleTo - angleFrom) / nbPoints - 90f);
						pointsArc[i] = center + new Godot.Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radius;
					}

					DrawPolygon(pointsArc, colors);
					break;
				case PolygonType.Triangle : 
					//Draw Triangle
					var trianglePoints = new Godot.Vector2[3];
					trianglePoints[0] = new Godot.Vector2(0,10);
					trianglePoints[1] = new Godot.Vector2(200,10);
					trianglePoints[2] = new Godot.Vector2(100,-100);
					DrawPolygon(trianglePoints, colors);
					break;
				case PolygonType.Square :
					/*
					var squarePoints = new Godot.Vector2[4];
					squarePoints[0] = new Godot.Vector2(10,0);
					squarePoints[1] = new Godot.Vector2(-10,0);
					squarePoints[2] = new Godot.Vector2(10,10);
					squarePoints[3] = new Godot.Vector2(-10,10);

					DrawPolygon(squarePoints, colors);
					*/
					Vector2[] points = new Vector2[]
					{
						new Vector2(100, 100), // Top-left corner
						new Vector2(200, 100), // Top-right corner
						new Vector2(200, 200), // Bottom-right corner
						new Vector2(100, 200) // Bottom-left corner
					};
						
					DrawPolygon(points, new Color[] { color });
					break;
				default: throw new NullReferenceException(); 
			}
			//for (int i = 0; i < nbPoints - 1; ++i)
			//	DrawLine(pointsArc[i], pointsArc[i + 1], new Color(255,215,0));
/*			if (drawSquare)
			{
				// Draw a square at the center
				float size = 100;
				var halfSize = size / 2;
				var rect = new Rect2(-halfSize, -halfSize, size, size);
				DrawRect(rect, Colors.Blue);
			}
			else
			{
				// Draw a circle at the center
				DrawCircle(Godot.Vector2.Zero, 50, Colors.Red);
			}*/
		}

		private void OnTimerTimeout()
		{
			drawSquare = true;
			Update(); // Trigger the redraw to update the shape
		}

		public override void _Process(float delta)
		{
			Update(); // Continuously redraw to reflect changes
		}
	}
}

/*	public class GoldenNugget : Polygon2D
	{
		private bool drawSquare = false;
		private Sprite sprite;
		private Polygon2D polygon;
		public PolygonType polygonType;
		public void setPolygonType(PolygonType _polygonType){
			polygonType = _polygonType;
		}
		public override void _Ready()
		{
			//sprite = GetNode<Sprite>("Sprite");
			//polygon = GetNode<Polygon2D>("Polygon2D");
			setPolygonType(PolygonType.Circle);
			_Draw();

			// Set up and start the Timer
			var timer = new Godot.Timer();
			timer.WaitTime = 5.0f; // 5 seconds
			timer.OneShot = true;  // The timer will only trigger once
			timer.Connect("timeout", this, nameof(OnTimerTimeout));
			AddChild(timer);
			timer.Start();

		}

		private void OnTimerTimeout()
		{
			drawSquare = true;
			Update(); // Trigger the redraw to update the shape
		}

		public override void _Draw()
		{
			var color = new Color(255,215,0);
			switch(polygonType){
				case PolygonType.Circle :
					//Draw Circle
					var center = new Godot.Vector2(50,50);
					var radius = 20;
					
					DrawCircle(center,radius,color);
					break;
				case PolygonType.Triangle : 
					//Draw Triangle
					break;
				case PolygonType.Square :
					var rect = new Rect2(new Godot.Vector2(50,50), new Godot.Vector2(20,20));
					DrawRect(rect,Color);
					break;
				default: throw new NullReferenceException(); 
			}
		}
		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			setPolygonType(PolygonType.Square);
			_Draw();
			Update();
		}

		
	}
}	
*/
