using Godot;
using System;

enum Crystal {
	Green,
	Red,
	Black,
	Blue,
	None
}
public partial class ExitDiasManager : Node
{
	[ExportGroup("Sockets")]
	[Export] public PlacementMonitor CrystalGreen;
	[Export] public PlacementMonitor CrystalRed;
	[Export] public PlacementMonitor CrystalBlack;
	[Export] public PlacementMonitor CrystalBlue;

	[ExportGroup("ButtonSequence")]
	[Export] public Sequence ButtonSequence;

	[Signal] public delegate void CrystalsPlacedEventHandler();
	[Signal] public delegate void CompletedEventHandler();

	private Godot.Collections.Array<bool> crystalsPlaced;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		crystalsPlaced = new Godot.Collections.Array<bool>();

		CrystalGreen.SetLockedKey("CrystalGreen");
		CrystalRed.SetLockedKey("CrystalRed");
		CrystalBlack.SetLockedKey("CrystalBlack");
		CrystalBlue.SetLockedKey("CrystalBlue");

		CrystalGreen.ItemPlaced += CrystalPlaced;
		CrystalRed.ItemPlaced += CrystalPlaced;
		CrystalBlack.ItemPlaced += CrystalPlaced;
		CrystalBlue.ItemPlaced += CrystalPlaced;

		//TODO Effects on placement

		ButtonSequence.Completed += ButtonSequenceCompleted;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	private void CrystalPlaced(bool correctItem, PlacementMonitor socket)
	{
		Crystal crystal = GetCrystal(socket);
		switch (crystal)
		{
			case Crystal.Green:
				crystalsPlaced.Add(true);
				break;
			case Crystal.Red:
				crystalsPlaced.Add(true);
				break;
			case Crystal.Black:
				crystalsPlaced.Add(true);
				break;
			case Crystal.Blue:
				crystalsPlaced.Add(true);
				break;
			default:
				GD.PushError("ERROR: Unhandled Crystal: ", crystal.ToString());
				break;
		} 

		if(crystalsPlaced.Count >= 4) EmitSignal(SignalName.CrystalsPlaced);
	}

	private Crystal GetCrystal(PlacementMonitor socket)
	{
		if(socket == CrystalGreen) return Crystal.Green;
		if(socket == CrystalRed) return Crystal.Red;
		if(socket == CrystalBlack) return Crystal.Black;
		if(socket == CrystalBlue) return Crystal.Blue;
		GD.PushError("ERROR: Unknown Crystal: ", socket.Name);
		return Crystal.None;
	}

	private void ButtonSequenceCompleted()
	{
		EmitSignal(SignalName.Completed);
	}
}
