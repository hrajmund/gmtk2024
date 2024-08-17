using Godot;
using System;
using Gmtk2024.Model;
using System.Numerics;
using System.Threading;

namespace Gmtk2024.Scripts{
	public class GoldenNugget : Polygon2D
	{
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
		}

        
	}
    

}
