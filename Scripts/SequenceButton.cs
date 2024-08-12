using Godot;
using System;
using System.Collections.Generic;



public partial class SequenceButton : Node3D
{
	[Signal]
	public delegate void ButtonPressEventHandler(SequenceButton sequenceButton);
	[ExportGroup("Internal Setup")]
	[Export] Interactable interactable;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Use for code you don't want running in editor
		if (!Engine.IsEditorHint()){
			// Call _Ready from the base class if you override it
			// Also call _GetConfigurationWarnings if you override it
			interactable.InteractionBegin += Begin;
			interactable.InteractionEnd += End;
		}
	}
	public override void _Process(double delta)
	{
	}
	// Called when parent interactable is interacted with
	public void Begin()
	{
		EmitSignal(SignalName.ButtonPress, this);
	}
	// Called when interaction ended
	public void End()
	{
		
	}
}

