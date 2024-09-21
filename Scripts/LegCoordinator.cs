using Godot;
using System;
using LegArray = Godot.Collections.Array<Leg>;

public partial class LegCoordinator : Node3D
{
	[Export] public LegArray LegQueue = new LegArray();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Leg leg in LegQueue)
		{
			leg.Setup(this);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		for (int i = 0; i<LegQueue.Count; i++){
			if (LegQueue[i].TryMove())
			{
				LegQueue.RemoveAt(i);
				i--;
			}
		}
	}
	public void completeStep(Leg leg){
		LegQueue.Add(leg);
	}
}
