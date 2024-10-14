using Godot;
using System;

public partial class BrainWorldManager : Node
{
	[ExportGroup("Object References")]
	[ExportSubgroup("Flags")]
	[Export] public Node3D imagePuzzleCover;
	[Export] public Node3D simonSaysCover;

	[ExportSubgroup("Fog & Neuron References")]
	[Export] public FogManager FogManager;
	[Export] public PlacementMonitor powerSocketDias;
	[Export] public PlacementMonitor powerSocketMaze;
	[Export] public PlacementMonitor powerSocketMiddle;
	[Export] public PlacementMonitor powerSocketBridge;
	[Export] public PlacementMonitor powerSocketTree;
	[Export] public PlacementMonitor powerSocketWater;



	[ExportGroup("Listeners")]
	[Export] public ImagePuzzleManager imagePuzzle;
	[Export] public SimonSaysManager simonSays;
	[Export] public ClutterRoomManager clutterRoom;
	[Export] public ExitDiasManager exitDias;

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
		clutterRoom.Completed += clutterRoomCompleted;
		exitDias.Completed += exitDiasCompleted;

		exitDias.CrystalsPlaced += crystalsPlaced;

		powerSocketDias.ItemPlaced += powerSocketFilled;
		powerSocketMaze.ItemPlaced += powerSocketFilled;
		powerSocketMiddle.ItemPlaced += powerSocketFilled;
		powerSocketBridge.ItemPlaced += powerSocketFilled;
		powerSocketTree.ItemPlaced += powerSocketFilled;
		powerSocketWater.ItemPlaced += powerSocketFilled;

	}

	private void powerSocketFilled(bool correctItem, PlacementMonitor socket)
	{
		if(socket == powerSocketDias) FogManager.RemoveFog(FogLocation.maze1);
		else if(socket == powerSocketMaze) FogManager.RemoveFog(FogLocation.maze2);
		else if(socket == powerSocketMiddle) FogManager.RemoveFog(FogLocation.clutterAndSimon);
		else if(socket == powerSocketBridge) FogManager.RemoveFog(FogLocation.bridge);
		else if(socket == powerSocketTree) FogManager.RemoveFog(FogLocation.tree);
		//else if(socket == powerSocketWater) river.QueueFree(); 
		else GD.PushError("Unhandled Power Socket: " + socket.Name);
	}


	private void imagePuzzleCompleted()
	{
		GD.Print("Image Puzzle Completed");
		//imagePuzzleFlag.Open();
		imagePuzzleCover.QueueFree();
		imagePuzzleDone = true;

		imagePuzzle.setActive(false);
	}

	private void simonSaysCompleted()
	{
		GD.Print("Simon Says Completed");
		//simonSaysFlag.Open();
		simonSaysCover.QueueFree();
		simonSaysDone = true;
		
		simonSays.setActive(false);
	}

	private void clutterRoomCompleted()
	{
		GD.Print("Clutter Room Completed");
		//clutterRoomFlag.Open();
		//clutterRoomCover.QueueFree();
		clutterRoomDone = true;
	}

	private void exitDiasCompleted()
	{
		//TODO end game
		GD.PrintRich("[shake rate=20.0 level=5 connected=1][rainbow]Game Completed![/rainbow][/shake]");
	}

	private void crystalsPlaced()
	{
		GD.Print("All Crystals Placed");
		//waterCover.QueueFree();
	}
}
