using Godot;
using System;
using System.Collections.Generic;

public class ShapeNormalization : Node
{
	public static List<float> NormalizeCircleData(List<float> circleData)
	{
		List<float> normalizedData = new List<float>();
		float maxRadius = Mathf.Max(circleData[0], circleData[1]);
		for (int i = 0; i < 2; i++)
		{
			normalizedData[i] = circleData[i] / maxRadius;
		}
		return normalizedData;
	}
	
	public static List<float> NormalizeTriangle(List<float> trianglePoints)
	{
		float cx = (trianglePoints[0] + trianglePoints[2] + trianglePoints[4]) / 3f;
		float cy = (trianglePoints[1] + trianglePoints[3] + trianglePoints[5]) / 3f;
		List<float> translatedPoints = new List<float> {
			trianglePoints[0] - cx, trianglePoints[1] - cy,
			trianglePoints[2] - cx, trianglePoints[3] - cy,
			trianglePoints[4] - cx, trianglePoints[5] - cy
		};
		float maxLength = Mathf.Max(
			Mathf.Max(
				Mathf.Sqrt(Mathf.Pow(translatedPoints[0] - translatedPoints[2], 2) + Mathf.Pow(translatedPoints[1] - translatedPoints[3], 2)),
				Mathf.Sqrt(Mathf.Pow(translatedPoints[2] - translatedPoints[4], 2) + Mathf.Pow(translatedPoints[3] - translatedPoints[5], 2))
			),
			Mathf.Sqrt(Mathf.Pow(translatedPoints[4] - translatedPoints[0], 2) + Mathf.Pow(translatedPoints[5] - translatedPoints[1], 2))
		);
		for (int i = 0; i < translatedPoints.Count; i++) {
			translatedPoints[i] /= maxLength;
		}
		return translatedPoints;
	}
	
	public static List<float> NormalizeSquare(List<float> squarePoints)
	{
		float cx = (squarePoints[0] + squarePoints[2] + squarePoints[4] + squarePoints[6]) / 4f;
		float cy = (squarePoints[1] + squarePoints[3] + squarePoints[5] + squarePoints[7]) / 4f;
		List<float> translatedPoints = new List<float> {
			squarePoints[0] - cx, squarePoints[1] - cy,
			squarePoints[2] - cx, squarePoints[3] - cy,
			squarePoints[4] - cx, squarePoints[5] - cy,
			squarePoints[6] - cx, squarePoints[7] - cy
		};
		float diagonalLength = Mathf.Sqrt(
			Mathf.Pow(translatedPoints[0] - translatedPoints[4], 2) +
			Mathf.Pow(translatedPoints[1] - translatedPoints[5], 2)
		);
		for (int i = 0; i < translatedPoints.Count; i++) {
			translatedPoints[i] /= diagonalLength;
		}
		return translatedPoints;
	}
}
