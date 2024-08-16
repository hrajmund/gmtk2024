using Godot;
using System;

public class GoldenNugget : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    // Called when the node enters the scene tree for the first time.
    private Sprite sprite;
    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        ApplyScale(new Vector2(5,0));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
