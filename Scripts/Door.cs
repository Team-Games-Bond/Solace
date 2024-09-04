using Godot;
using System;

public partial class Door : Node3D
{
	[Export] float moveDistance = 3;
	public void Open()
	{
		this.TranslateObjectLocal(Vector3.Up * moveDistance);
	}

	public void Close()
	{
		this.TranslateObjectLocal(Vector3.Down * moveDistance);
	}
}
