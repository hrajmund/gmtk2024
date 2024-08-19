using Godot;
using System;

public class MessageBubbleLabel : Label
{
    private string[] _positiveMessages = { "Shiny", "Treasure", "Beauty", "Me like", "Mine" };
    private string[] _negativeMessages = { "Tarnished tin", "Looks like an old boot", 
        "I'd rather see me own reflection", "This gold could scare a troll", 
        "I’d trade it for moldy bread", };


    public void SetRandomPositive()
    {
        Text = _positiveMessages[GD.Randi() % _positiveMessages.Length] + '!';
    }

    public void SetRandomNegative()
    {
        Text = _negativeMessages[GD.Randi() % _negativeMessages.Length] + '!';
    }
}
