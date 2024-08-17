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
		public float[] circleData;

		public override void _Ready()
		{
			colors = new Color[] { color };
			circleData = new float[3];
			setPolygonType(PolygonType.Circle,2,144,121);

			// 360 - playerInput
			// playerInput 
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

					float rotationAngle = 360.0f - circleData[2];
					float rotationRad = Mathf.Deg2Rad(rotationAngle);

					for (int i = 0; i < nbPoints; ++i)
					{
						float anglePoint = Mathf.Deg2Rad(angleFrom + i * (angleTo - angleFrom) / nbPoints - 90);
						float x = Mathf.Cos(anglePoint) * circleData[0];
						float y = Mathf.Sin(anglePoint) * circleData[1];

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
		public void updateSquareAddition(float value, Dimension dimension){
			for(int i = 0; i < squarePointTable.Count; i++){
				squarePointTable[i] += squarePointTable[i]*value;	
			}
		}
		public void updateSquareDivison(float value, Dimension dimension){
			for(int i = 0; i < squarePointTable.Count; i++){
				squarePointTable[i] /= value;	
			}
		}
		public void updateSquareMultiplication(float value, Dimension dimension){
			for(int i = 0; i < squarePointTable.Count; i++){
				squarePointTable[i] *= value;	
			}
		}
		public void updateSquareSubstraction(float value, Dimension dimension){
			for(int i = 0; i < squarePointTable.Count; i++){
				squarePointTable[i] -= squarePointTable[i]*value;	
			}
		}
		public void updateSquarePower(float value, Dimension dimension){
			for(int i = 0; i < squarePointTable.Count; i++){
				squarePointTable[i] = Mathf.Pow(squarePointTable[i],value);	
			}
		}
		public void updateSquareRoot(float value, Dimension dimension){
			for(int i = 0; i < squarePointTable.Count; i++){
				squarePointTable[i] = Mathf.Pow(squarePointTable[i],1/value);	
			}
		}
		public void updateSquareRotation(float value, Dimension dimension){
			setPolygonType(PolygonType.Square,(int)value,0,0);	
		}
		public void updateCircleAddition(float value, Dimension dimension){
			if(circleData[0] == circleData[1]){
				circleData[0] += circleData[0]*value;
				circleData[1] += circleData[1]*value;
			}else{
				if(dimension == Dimension.X){
					circleData[0] += circleData[0]*value;
				}else{
					circleData[1] += circleData[1]*value;
				}
			}
		}
		public void updateCircleDivison(float value, Dimension dimension){
			if(circleData[0] == circleData[1]){
				circleData[0] /= value;
				circleData[1] /= value;
			}else{
				if(dimension == Dimension.X){
					circleData[0] /= value;
				}else{
					circleData[1] /= value;
				}
			}
		}
		public void updateCircleMultiplication(float value, Dimension dimension){
			if(circleData[0] == circleData[1]){
				circleData[0] *= value;
				circleData[1] *= value;
			}else{
				if(dimension == Dimension.X){
					circleData[0] *= value;
				}else{
					circleData[1] *= value;
				}
			}
		}
		public void updateCircleSubstraction(float value, Dimension dimension){
			if(circleData[0] == circleData[1]){
				circleData[0] -= circleData[0]*value;
				circleData[1] -= circleData[1]*value;
			}else{
				if(dimension == Dimension.X){
					circleData[0] -= circleData[0]*value;
				}else{
					circleData[1] -= circleData[1]*value;
				}
			}
		}
		public void updateCirclePower(float value, Dimension dimension){
			if(circleData[0] == circleData[1]){
				circleData[0] = Mathf.Pow(circleData[0],value);
				circleData[1] = Mathf.Pow(circleData[1],value);
			}else{
				if(dimension == Dimension.X){
					circleData[0] = Mathf.Pow(circleData[0],value);
				}else{
					circleData[1] = Mathf.Pow(circleData[1],value);
				}
			}
		}
		public void updateCircleRoot(float value, Dimension dimension){
			if(circleData[0] == circleData[1]){
				circleData[0] = Mathf.Pow(circleData[0],1/value);
				circleData[1] = Mathf.Pow(circleData[1],1/value);
			}else{
				if(dimension == Dimension.X){
					circleData[0] = Mathf.Pow(circleData[0],1/value);
				}else{
					circleData[1] = Mathf.Pow(circleData[1],1/value);
				}
			}
		}
		public void updateCircleRotation(float value, Dimension dimension){
			if(circleData[0] == circleData[1]){
				return;
			}else{
				setPolygonType(PolygonType.Circle,(int)value,(int)circleData[0],(int)circleData[1]);	
			}
		}
		public void updateTriangleAddition(float value, Dimension dimension){
			for(int i = 0; i < trianglePointTable.Count; i++){
				trianglePointTable[i] += trianglePointTable[i]*value;	
			}
		}
		public void updateTriangleDivison(float value, Dimension dimension){
			for(int i = 0; i < trianglePointTable.Count; i++){
				trianglePointTable[i] /= value;	
			}
		}
		public void updateTriangleMultiplication(float value, Dimension dimension){
			for(int i = 0; i < trianglePointTable.Count; i++){
				trianglePointTable[i] *= value;	
			}
		}
		public void updateTriangleSubstraction(float value, Dimension dimension){
			for(int i = 0; i < trianglePointTable.Count; i++){
				trianglePointTable[i] -= trianglePointTable[i]*value;	
			}
		}
		public void updateTrianglePower(float value, Dimension dimension){
			for(int i = 0; i < trianglePointTable.Count; i++){
				trianglePointTable[i] = Mathf.Pow(trianglePointTable[i],value);	
			}
		}
		public void updateTriangleRoot(float value, Dimension dimension){
			for(int i = 0; i < trianglePointTable.Count; i++){
				trianglePointTable[i] = Mathf.Pow(trianglePointTable[i],1/value);	
			}
		}
		public void updateTriangleRotation(float value, Dimension dimension){
			for(int i = 0; i < trianglePointTable.Count; i++){
				setPolygonType(PolygonType.Triangle,(int)value,0,0);	
			}			
		}
		public void updatePolygon(Operation operation, float value, Dimension dimension){
			switch(operation){
				case Operation.Addition: {
					if(polygonType == PolygonType.Square){
						updateSquareAddition(value, dimension);			
					}else if (polygonType == PolygonType.Circle){
						updateCircleAddition(value, dimension);
					}else if (polygonType == PolygonType.Triangle){
						updateTriangleAddition(value, dimension);
					}
					break;
				}
				case Operation.Division: {
					if(polygonType == PolygonType.Square){
						updateSquareDivison(value, dimension);			
					}else if (polygonType == PolygonType.Circle){
						updateCircleDivison(value, dimension);
					}else if (polygonType == PolygonType.Triangle){
						updateTriangleDivison(value, dimension);
					}
					break;
				}
				case Operation.Multiplication: {
					if(polygonType == PolygonType.Square){
						updateSquareMultiplication(value, dimension);			
					}else if (polygonType == PolygonType.Circle){
						updateCircleMultiplication(value, dimension);
					}else if (polygonType == PolygonType.Triangle){
						updateTriangleMultiplication(value, dimension);
					}					
					break;
				}
				case Operation.Subtraction: {
					if(polygonType == PolygonType.Square){
						updateSquareSubstraction(value, dimension);			
					}else if (polygonType == PolygonType.Circle){
						updateCircleSubstraction(value, dimension);
					}else if (polygonType == PolygonType.Triangle){
						updateTriangleSubstraction(value, dimension);
					}		
					break;
				}
				case Operation.Power: {
					if(polygonType == PolygonType.Square){
						updateSquarePower(value, dimension);			
					}else if (polygonType == PolygonType.Circle){
						updateCirclePower(value, dimension);
					}else if (polygonType == PolygonType.Triangle){
						updateTrianglePower(value, dimension);
					}
					break;
				}
				case Operation.Root: {
					if(polygonType == PolygonType.Square){
						updateSquareRoot(value, dimension);			
					}else if (polygonType == PolygonType.Circle){
						updateCircleRoot(value, dimension);
					}else if (polygonType == PolygonType.Triangle){
						updateTriangleRoot(value, dimension);
					}
					break;
				}
				case Operation.Rotation:{
					if(polygonType == PolygonType.Square){
						updateSquareRotation(value, dimension);			
					}else if (polygonType == PolygonType.Circle){
						updateCircleRotation(value, dimension);
					}else if (polygonType == PolygonType.Triangle){
						updateTriangleRotation(value, dimension);
					}
					break;
				}
			}
			Update();
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
				circleData[0] = radiusA;
				circleData[1] = radiusB;
				circleData[2] = 45;
//				circleData[2] = (float)rotateType;
			}
		}
	} 
}
