using Godot;
using System;

public partial class ZoomChange : Node
{
	[Export] public float distance;
	[Export] public bool overideWithStartDistance = false;
	[Export] public bool timedZoom = false;
	[Export] public double timedZoomLength = 10;

	[ExportGroup("Setup Settings")]
	[Export] public Area3D area;
	[Export] public Timer zoomTimer;
	[Export] public bool isActive = true;

	private CameraController controller;
	private float previousDistance;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!Engine.IsEditorHint()){
			// Makes the assumption that there is only one camera controller in a scene
			controller = (CameraController)GetTree().GetFirstNodeInGroup("Character Controllers");

			zoomTimer.WaitTime = timedZoomLength;

			zoomTimer.Timeout += returnZoom;
			area.BodyEntered += EnteredZone;

			if(overideWithStartDistance) distance = controller.StartDistance;
		}
	}

	public void EnteredZone(Node3D body)
	{
		if(!isActive) return;

		CharacterController player = (CharacterController)body;

		if(timedZoom && zoomTimer.IsStopped()) 
		{
			previousDistance = controller.GetDistance();
			zoomTimer.Start();
		} else if(timedZoom && !zoomTimer.IsStopped()) zoomTimer.Start();

		controller.ZoomCamera(distance);
	}

	public void returnZoom()
	{
		if(!isActive) return;

		controller.ZoomCamera(previousDistance);
		zoomTimer.Stop();
	}
}
