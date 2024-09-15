using Godot;
using System;

public enum FogLocation
{
	maze1,
	maze2,
	clutterAndSimon,
	bridge,
	tree
}

public partial class FogManager : Node
{
	[Export] public Node3D mazeFog1;
	[Export] public Node3D mazeFog2;
	[Export] public Node3D clutterAndSimonFog;
	[Export] public Node3D bridgeFog;
	[Export] public Node3D treeFog;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RemoveFog(FogLocation fog)
	{
		switch (fog)
		{
			case FogLocation.maze1:
				mazeFog1.QueueFree();
				return;
			case FogLocation.maze2:
				mazeFog2.QueueFree();
				return;
			case FogLocation.clutterAndSimon:
				clutterAndSimonFog.QueueFree();
				return;
			case FogLocation.bridge:
				bridgeFog.QueueFree();
				return;
			case FogLocation.tree:
				treeFog.QueueFree();
				return;
			default:
				GD.PushError("Unhandled Fog Location: " + fog.ToString());
				return;
		}
	}

}
