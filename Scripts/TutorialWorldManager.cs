using Godot;
using System;

public partial class TutorialWorldManager : Node
{
	[ExportGroup("Object References")]
	[Export] public Door room1Door;
	[Export] public Door room2Door;
	[Export] public Door room3Door;

	[ExportGroup("Listeners")]
	[Export] public Sequence room1Sequence;

	//Other variables


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		room1Sequence.Completed += Room1Completed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Room1Completed()
	{
		GD.Print("Room 1 Completed");
		room1Door.Open();
	}
}
