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

	// Variables to track room completion
	private bool room1Done = false;
	private bool room2Done = false;
	private bool room3Done = false;

	// Background music player
	private AudioStreamPlayer bg_music;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Entering _Ready method...");

		// Attempt to get the AudioStreamPlayer node using the relative path
		bg_music = GetNode<AudioStreamPlayer>("../bg_music");
		if (bg_music != null)
		{
			GD.Print("AudioStreamPlayer 'bg_music' found. Attempting to play...");
			bg_music.Play();
		}
		else
		{
			GD.Print("AudioStreamPlayer 'bg_music' not found!");
		}

		room1Sequence.Completed += Room1Completed;
		room2PowerSocket.ItemPlaced += Room2SocketHandler;
		room2PowerSocket.ItemRemoved += Room2SocketHandler;
		room3Sequence.Completed += Room3Completed;
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

	public void Room2SocketHandler(bool isCorrect, PlacementMonitor monitor)
	{
		if (isCorrect)
		{
			Room2Completed();
		}
	}

	public void Room2SocketHandler()
	{
		if (room2Done)
		{
			room2Door.Close();
			room2Done = false;
		}
	}
}
