using Godot;
using System;

public partial class ZoomOnPress : Node
{
	private CameraController cameraController;
	[Export] float zoomAmount = 1f;
	[Export] Sequence sequence;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!Engine.IsEditorHint()){
			// Makes the assumption that there is only one camera controller in a scene
			cameraController = (CameraController)GetTree().GetFirstNodeInGroup("Character Controller");
			//GD.Print((CameraController)GetTree().GetFirstNodeInGroup("Character Controller"));

			sequence.Correct += ZoomIn;
			sequence.Incorrect += ZoomOut;
		}
	}

	void ZoomIn(){
		cameraController.ZoomCameraRelative(zoomAmount);
	}
	void ZoomOut(){
		cameraController.ZoomCameraRelative(-zoomAmount);
	}
}
