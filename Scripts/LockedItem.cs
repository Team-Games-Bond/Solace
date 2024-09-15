using Godot;
using System;

public partial class LockedItem : Node3D
{
	[ExportGroup("Sockets")]
	[Export] public Godot.Collections.Array<PlacementMonitor> Sockets { get; set; }
	[Export] public Godot.Collections.Array<string> ItemKeys { get; set; }

	[ExportGroup("Buttons")]
	[Export] public Godot.Collections.Array<Button> Buttons { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(ItemKeys != null)
		{
			for(int i = 0; i < Sockets.Count; i++)
			{
				Sockets[i].SetLockedKey(ItemKeys[i]);
			}
		}

		if(Sockets != null)
		{
			foreach (var socket in Sockets)
			{
				socket.setSocketActive(false);
			}
		}
		if(Buttons != null)
		{
			foreach (var button in Buttons)
			{
				button.SetActive(false);
			}
		}

	}

	public void Activate()
	{
		if(Sockets != null)
		{
			foreach (var socket in Sockets)
			{
				socket.setSocketActive(true);
			}
		}
		if(Buttons != null)
		{
			foreach (var button in Buttons)
			{
				button.SetActive(true);
			}
		}
	}
}
