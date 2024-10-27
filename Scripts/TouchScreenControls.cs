using Godot;
using System;

public partial class TouchScreenControls : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		VisibilityCheck();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void VisibilityCheck(){
		Visible = DisplayServer.IsTouchscreenAvailable()&!GetTree().Paused;
	}
}
