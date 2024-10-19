using Godot;
using System;
using System.Transactions;

public partial class NeuronWiggle : Node
{
	[Export] float minTime = 0;
	[Export] float maxTime = 1;

	[ExportGroup("Settings")]
	[Export] Timer timer;
	[Export] AnimationPlayer animationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationPlayer.Pause();
		GD.Randomize();
		timer.WaitTime = GD.RandRange(minTime, maxTime);

		animationPlayer.SpeedScale = (float)GD.RandRange(0.05, 0.8);

		timer.Timeout += StartAnimation;

		timer.Start();
	}

	public void StartAnimation()
	{
		GD.Print("Started: " + timer.WaitTime);
		if(GD.Randi() % 2 == 1) animationPlayer.Play();
		else animationPlayer.PlayBackwards();
	}
}
