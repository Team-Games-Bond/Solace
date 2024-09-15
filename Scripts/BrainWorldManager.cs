using Godot;
using System;

public partial class BrainWorldManager : Node
{
	[ExportGroup("Object References")]
	[Export] public Door imagePuzzleFlag;
	[Export] public Door simonSaysFlag;
	[Export] public Door clutterRoomFlag;

	[ExportGroup("Listeners")]
	[Export] public ImagePuzzleManager imagePuzzle;
	[Export] public SimonSaysManager simonSays;
	[Export] public ClutterRoomManager clutterRoom;

	//Other variables
	bool imagePuzzleDone = false;
	bool simonSaysDone = false;
	bool clutterRoomDone = false;
	bool exitDiasDone = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		imagePuzzle.Completed += imagePuzzleCompleted;
		simonSays.Completed += simonSaysCompleted;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void imagePuzzleCompleted()
	{
		GD.Print("Image Puzzle Completed");
		imagePuzzleFlag.Open();
		imagePuzzleDone = true;

		imagePuzzle.setActive(false);
	}

	private void simonSaysCompleted()
	{
		GD.Print("Simon Says Completed");
		simonSaysFlag.Open();
		simonSaysDone = true;
		
		simonSays.setActive(false);
	}

	private void clutterRoomCompleted()
	{
		GD.Print("Clutter Room Completed");
		clutterRoomFlag.Open();
		clutterRoomDone = true;
	}
}
