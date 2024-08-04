using Godot;
using System;

public partial class CameraController : Camera3D
{
	[ExportGroup("Controller Settings")]
	[Export] public float StartDistance = 12f;
	[Export] public float MaxDistance = 24f;
	[Export] public float MinDistance = 5f;

	[ExportGroup("Controller Setup")]
	[Export] public CharacterController CharacterController;

	//Other Variables
	private float targetDistance;
	private Camera3D currentCamera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Size = StartDistance;
		targetDistance = StartDistance;
		currentCamera = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Don't process if in puzzle mode
		if(CharacterController.isPuzzleMode) return;

		GD.Print(delta);

		Size = (float)Mathf.Lerp(Size, targetDistance, delta);
	}

	public void ZoomCamera(float distance) //TODO-----------------------------
	{
		//Bound by max-min
	}

	public void ChangeCamera(Camera3D newCamera) //TODO-----------------------------
	{
		//Do changing from currentCamera to newCamera

		currentCamera = newCamera;
	}

	public void ChangeCamera() //TODO-----------------------------
	{
		//Do changing from currentCamera to this camera

		currentCamera = this;	
	}

}
