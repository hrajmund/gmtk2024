using Godot;
using System;
using Gmtk2024.Model;
using System.Numerics;
using System.Threading;
using System.Collections.Generic;
using Vector2 = Godot.Vector2;

namespace Gmtk2024.Scripts{
	public class GoldenNugget : Node2D
	{
		private Color color = new Color(255,215,0);
		private Color[] colors;
		public PolygonType polygonType;
		public List<float> trianglePointTable = new List<float>();
		public List<float> squarePointTable = new List<float>();
		public float[] radiuses;

		public override void _Ready()
		{
			colors = new Color[] { color };
			radiuses = new float[2];
			setPolygonType(PolygonType.Circle,0,144,78);
		}

		public override void _Draw()
		{
			switch(polygonType){
				case PolygonType.Circle :
					//Draw Circle
					int nbPoints = 360;
					var pointsArc = new Godot.Vector2[nbPoints];
					Godot.Vector2 center = new Godot.Vector2(0,0);
					
					
					float angleTo = 360.0f;
					float angleFrom = 0.0f;
/*
					for (int i = 0; i < nbPoints; ++i)
					{
						float anglePoint = Mathf.Deg2Rad(angleFrom + i * (angleTo - angleFrom) / nbPoints - 90);
						float x = Mathf.Cos(anglePoint) * radiuses[0];
						float y = Mathf.Sin(anglePoint) * radiuses[1];
						pointsArc[i] = center + new Vector2(x, y);
					}*/

						// Convert the rotation angle to radians
					float fasz = 90.0f;
					float rotationAngle = 360.0f - fasz;
					float rotationRad = Mathf.Deg2Rad(rotationAngle);

					for (int i = 0; i < nbPoints; ++i)
					{
						float anglePoint = Mathf.Deg2Rad(angleFrom + i * (angleTo - angleFrom) / nbPoints - 90);
						float x = Mathf.Cos(anglePoint) * radiuses[0];
						float y = Mathf.Sin(anglePoint) * radiuses[1];

						// Apply rotation using the 2D rotation matrix
						float rotatedX = x * Mathf.Cos(rotationRad) - y * Mathf.Sin(rotationRad);
						float rotatedY = x * Mathf.Sin(rotationRad) + y * Mathf.Cos(rotationRad);

						pointsArc[i] = center + new Vector2(rotatedX, rotatedY);
					}

					DrawPolygon(pointsArc, colors);
					break;
				case PolygonType.Triangle : 
					//Draw Triangle
					var trianglePoints = new Godot.Vector2[3];
					trianglePoints[0] = new Godot.Vector2(trianglePointTable[0],trianglePointTable[1]);
					trianglePoints[1] = new Godot.Vector2(trianglePointTable[2],trianglePointTable[3]);
					trianglePoints[2] = new Godot.Vector2(trianglePointTable[4],trianglePointTable[5]);
					DrawPolygon(trianglePoints, colors);
					break;
				case PolygonType.Square :
					Vector2[] points = new Vector2[]
					{
						new Vector2(squarePointTable[0], squarePointTable[1]), // Top-left corner
						new Vector2(squarePointTable[2], squarePointTable[3]), // Top-right corner
						new Vector2(squarePointTable[4], squarePointTable[5]), // Bottom-right corner
						new Vector2(squarePointTable[6], squarePointTable[7]) // Bottom-left corner
					};
						
					DrawPolygon(points, colors);
					break;
				default: throw new NullReferenceException(); 
			}
		}

		public override void _Process(float delta)
		{
			Update(); // Continuously redraw to reflect changes
		}
		public void setPolygonType(PolygonType _polygonType, int rotateType = 1, int radiusA = 0, int radiusB = 0){
			polygonType = _polygonType;
			if (polygonType == PolygonType.Triangle){
				switch(rotateType){
					case 1:
					{
						trianglePointTable.Add(-100); trianglePointTable.Add(100); 		//A
						trianglePointTable.Add(100); trianglePointTable.Add(100);  		//B
						trianglePointTable.Add(0); trianglePointTable.Add(-100);   		//C
						break;
					}
					case 2:
					{
						trianglePointTable.Add(-100); trianglePointTable.Add(-100);		//A
						trianglePointTable.Add(100); trianglePointTable.Add(100); 		//B
						trianglePointTable.Add(100); trianglePointTable.Add(-100); 		//C
						break;
					}
					case 3:
					{
						trianglePointTable.Add(-100); trianglePointTable.Add(0); 		//A
						trianglePointTable.Add(100); trianglePointTable.Add(100); 		//B
						trianglePointTable.Add(100); trianglePointTable.Add(-100); 		//C
						break;
					}
					case 4: 
					{
						trianglePointTable.Add(-100); trianglePointTable.Add(-100); 	//A
						trianglePointTable.Add(100); trianglePointTable.Add(100); 		//B
						trianglePointTable.Add(100); trianglePointTable.Add(-100); 		//C
						break;
					}
					case 5:
					{
						trianglePointTable.Add(-100); trianglePointTable.Add(-100); 	//A
						trianglePointTable.Add(0); trianglePointTable.Add(100); 		//B
						trianglePointTable.Add(100); trianglePointTable.Add(-100); 		//C
						break;
					}
					case 6:
					{
						trianglePointTable.Add(-100); trianglePointTable.Add(-100); 	//A
						trianglePointTable.Add(-100); trianglePointTable.Add(100); 		//B
						trianglePointTable.Add(100); trianglePointTable.Add(-100); 		//C
						break;
					}
					case 7:
					{
						trianglePointTable.Add(-100); trianglePointTable.Add(-100); 	//A
						trianglePointTable.Add(-100); trianglePointTable.Add(100); 		//B
						trianglePointTable.Add(100); trianglePointTable.Add(0); 		//C
						break;
					}
					case 8:
					{
						trianglePointTable.Add(-100); trianglePointTable.Add(-100); 	//A
						trianglePointTable.Add(-100); trianglePointTable.Add(100); 		//B
						trianglePointTable.Add(100); trianglePointTable.Add(100); 		//C
						break;
					}
					default: break;
				}
			}else if(polygonType == PolygonType.Square){
				switch(rotateType)
				{
					case 1:
					{
						squarePointTable.Add(-100); squarePointTable.Add(-100); 		//A
						squarePointTable.Add(100); squarePointTable.Add(-100); 			//B
						squarePointTable.Add(100); squarePointTable.Add(100); 			//C
						squarePointTable.Add(-100); squarePointTable.Add(100); 			//D						
						break;
					}
					case 2:
					{
						squarePointTable.Add(0); squarePointTable.Add(-100); 			//A
						squarePointTable.Add(100); squarePointTable.Add(0); 			//B
						squarePointTable.Add(0); squarePointTable.Add(100); 			//C
						squarePointTable.Add(-100); squarePointTable.Add(0); 			//D
						break;
					}
					default: break;
				}
			}else{
				radiuses[0] = radiusA;
				radiuses[1] = radiusB;
			}
		}
	} 
}
