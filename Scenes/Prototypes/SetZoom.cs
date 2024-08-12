using Godot;
using System;

public partial class SetZoom : Node
{
	private CameraController cameraController;
	[Export] float zoomClose = 1f;
	[Export] float zoomMid = 15f;
	[Export] float zoomFar = 200f;
	[Export] SequenceButton zoomIn;
	[Export] SequenceButton zoomReset;
	[Export] SequenceButton zoomOut;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!Engine.IsEditorHint()){
			// Makes the assumption that there is only one camera controller in a scene
			cameraController = (CameraController)GetTree().GetFirstNodeInGroup("Character Controller");
			//GD.Print((CameraController)GetTree().GetFirstNodeInGroup("Character Controller"));

			zoomIn.ButtonPress += ZoomCamera;
			zoomOut.ButtonPress += ZoomCamera;
			zoomReset.ButtonPress += ZoomCamera;
		}
	}

	void ZoomCamera(SequenceButton sequenceButton){
		if(sequenceButton == zoomIn) cameraController.ZoomCamera(zoomClose);
		if(sequenceButton == zoomOut) cameraController.ZoomCamera(zoomFar);
		if(sequenceButton == zoomReset) cameraController.ZoomCamera(zoomMid);
	}
}
