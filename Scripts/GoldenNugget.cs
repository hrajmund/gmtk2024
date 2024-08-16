using Godot;
using System;
using Gmtk2024.Model;

namespace Gmtk2024.Scripts{
    public class GoldenNugget : Node2D
    {
        private Sprite sprite;
        private Polygon2D polygon;
        public override void _Ready()
        {
            sprite = GetNode<Sprite>("Sprite");
            polygon = GetNode<Polygon2D>("Polygon2D");
        }

        public void setPolygon(PolygonType polygonType){
            try {
                switch(polygonType){
                    case PolygonType.Circle : 
                        //Draw Circle
                        break;
                    case PolygonType.Triangle : 
                        //Draw Triangle
                        break;
                    case PolygonType.Square :
                        //Draw Square
                        break;
                    default: throw new NullReferenceException(); 
                }
            }catch (Exception e){
                Console.WriteLine(e.Message);
            }
        }



    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    }

}
