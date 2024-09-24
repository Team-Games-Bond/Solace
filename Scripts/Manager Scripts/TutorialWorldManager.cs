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
	[Export] public PlacementMonitor room2PowerSocket;
	[Export] public Sequence room3Sequence;

	//Other variables
	bool room1Done = false;
	bool room2Done = false;
	bool room3Done = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		room1Sequence.Completed += Room1Completed;
		room2PowerSocket.ItemPlaced += Room2SocketHandler;
		room2PowerSocket.ItemRemoved += Room2SocketHandler;
		room3Sequence.Completed += Room3Completed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Room1Completed()
	{
		GD.Print("Room 1 Completed");
		room1Door.Open();
		room1Done = true;
	}
	public void Room2Completed()
	{
		GD.Print("Room 2 Completed");
		room2Door.Open();
		room2Done = true;
	}
	public void Room3Completed()
	{
		GD.Print("Room 3 Completed");
		room3Door.Open();
		room3Done = true;
	}

	public void Room2SocketHandler(bool isCorrect, PlacementMonitor monotor) //Handle item added
	{
		if(isCorrect)
		{
			Room2Completed();
		}
		else
		{
			//Anything on fail
		}
	}
	public void Room2SocketHandler() //Handle item removed
	{
		if(room2Done)
		{
			room2Door.Close();
			room2Done = false;
		}
	}


}
