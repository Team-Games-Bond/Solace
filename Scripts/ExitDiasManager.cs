using Godot;
using System;

public partial class ExitDiasManager : Node
{
	[ExportGroup("Sockets")]
	[Export] public PlacementMonitor CrystalGreen;
	[Export] public PlacementMonitor CrystalRed;
	[Export] public PlacementMonitor CrystalBlack;
	[Export] public PlacementMonitor CrystalBlue;

	[ExportGroup("ButtonSequence")]
	[Export] public Sequence ButtonSequence;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CrystalGreen.SetLockedKey("CrystalGreen");
		CrystalRed.SetLockedKey("CrystalRed");
		CrystalBlack.SetLockedKey("CrystalBlack");
		CrystalBlue.SetLockedKey("CrystalBlue");

		//CrystalGreen.ItemPlaced += CrystalGreenPlaced;
		//CrystalRed.ItemPlaced += CrystalRedPlaced;
		//CrystalBlack.ItemPlaced += CrystalBlackPlaced;
		//CrystalBlue.ItemPlaced += CrystalBluePlaced;

		ButtonSequence.Completed += ButtonSequenceCompleted;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private void ButtonSequenceCompleted()
	{
		GD.PrintRich("[shake rate=20.0 level=5 connected=1][rainbow]Game Completed![/rainbow][/shake]");
		//TODO end game
	}
}
