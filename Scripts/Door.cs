using Godot;
using System;

public partial class Door : Node3D
{
	[Export] float moveUpDistance = 3;
	public void Open()
	{
		this.TranslateObjectLocal(Vector3.Up * moveUpDistance);
	}
}
