using Godot;
using System;

public partial class ClutterRoomManager : Node
{
	[ExportGroup("Sockets")]
	[Export] public Godot.Collections.Array<PlacementMonitor> Sockets { get; set; }
	[Export] public Godot.Collections.Array<string> LockKeys { get; set; }
	[ExportGroup("Blockers")]
	[Export] public Godot.Collections.Array<CsgBox3D> Blockers { get; set; }
	[Signal] public delegate void CompletedEventHandler();

	private Godot.Collections.Array<bool> socketsFilled;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for(int i = 0; i < Sockets.Count; i++)
		{
			Sockets[i].SetLockedKey(LockKeys[i]);
		}

		socketsFilled = new Godot.Collections.Array<bool>();
		for (int i = 0; i < Sockets.Count; i++)
		{
			socketsFilled.Add(false);
		}

		foreach (var socket in Sockets)
		{
			socket.ItemPlaced += ItemPlacedInSocket; //Subscribe to the socket
			socket.ItemRemoved += ItemRemovedFromSocket;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ItemPlacedInSocket(bool correctItem, PlacementMonitor monitor)
	{
		if(!correctItem) GD.PushError("Item Placed In Locked Socket was not correct");
		int socketIndex = GetSocketIndex(monitor);
		socketsFilled[socketIndex] = true;
		Blockers[socketIndex].UseCollision = false;

		CheckCompletion();
	}

	public void ItemRemovedFromSocket()
	{
		
	}

	private int GetSocketIndex(PlacementMonitor monitor)
	{
		for (int i = 0; i < Sockets.Count; i++)
		{
			if(Sockets[i] == monitor) return i;
		}
		GD.PushError(GetPath()," ERROR: Socket Not Found");
		return -1;
	}

	private void CheckCompletion()
	{
		if(isCompleted()) EmitSignal(SignalName.Completed);
	}

	private bool isCompleted()
	{
		foreach (var socketFilled in socketsFilled)
		{
			if(!socketFilled) return false;
		}
		return true;
	}
}
