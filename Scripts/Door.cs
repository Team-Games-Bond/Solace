using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime;

public partial class Door : Node3D
{
	[Export] float moveDistance = 3;
	[Export] float moveSpeed = 1f;
	[Export] bool startClosed = true;

	
	Vector3 openPosition;
	Vector3 closedPosition;

	private bool closing = true;

	private float moveValue = 1f;

    public override void _Ready()
    {
        openPosition = new Vector3(Position.X, Position.Y - moveDistance, Position.Z);
		closedPosition = Position;

		if(!startClosed) 
		{
			Position = openPosition;
			closing = false;
		} 
    }

    public override void _Process(double delta)
    {
		moveValue = Mathf.Min(1, moveValue + (float)delta * moveSpeed);
		if(closing)
		{
			Position = openPosition.Lerp(closedPosition, moveValue);
		} else
		{
			Position = closedPosition.Lerp(openPosition, moveValue);
		}
		
		//GD.Print(Position);
    }

	public void Open()
	{
		closing = false;
		moveValue = 0;
	}

	public void Close()
	{
		closing = true;
		moveValue = 0;
	}
}
