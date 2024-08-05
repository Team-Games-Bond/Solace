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
	private Camera3D currentCamera; //Not used currently, would be of use if we want to create an animation between cameras

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

		//GD.Print(delta);

		Size = (float)Mathf.Lerp(Size, targetDistance, delta);
	}

	public void ZoomCamera(float distance)
	{
		//Bound by max-min
		distance = Math.Max(Math.Min(distance, MaxDistance), MinDistance);
		targetDistance = distance;
	}

	public void ChangeCamera(Camera3D newCamera)
	{
		//Changing to newCamera
		newCamera.MakeCurrent();
		currentCamera = newCamera;
	}

	public void ChangeCamera() 
	{
		//Do changing to this camera
		this.MakeCurrent();
		currentCamera = this;	
	}

}
