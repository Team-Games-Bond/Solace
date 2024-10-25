using Godot;
using System;

public partial class Ending : Node
{
	[Export] public double fadeTime = 3;

	[ExportGroup("Setup Settings")]
	[Export] public Area3D area;
	[Export] public CanvasLayer canvas;
	[Export] public Control endScreen;
	[Export] public Control Credits;
	[Export] public Timer fadeTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
