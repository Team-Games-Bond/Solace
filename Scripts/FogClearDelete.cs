using Godot;
using System;
using System.ComponentModel.DataAnnotations.Schema;

public partial class FogClearDelete : Node
{
	[Export] FogLocation location;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void Delete(int toDelete){
		GD.Print("Checking to delete ", (FogLocation)toDelete, " in ", location);
		if((FogLocation)toDelete==location){
			QueueFree();
			GD.Print("Deleted ", location);
		}
	}
}
