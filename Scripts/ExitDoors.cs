using Godot;
using System;

public partial class ExitDoors : Node
{
	[Export] float speed = 1;
	[ExportGroup("Settings")]
	[Export] AnimationPlayer animationPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationPlayer.SpeedScale = speed;
		animationPlayer.Stop();
	}

	public void Open()
	{
		animationPlayer.Play();
	}
}
