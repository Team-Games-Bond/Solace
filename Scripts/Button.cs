using Godot;
using System;
using System.Collections.Generic;


[Tool]
public partial class Button : Node3D
{
	[Signal]
	public delegate void ButtonPressEventHandler(Button button);
	[ExportGroup("Internal Setup")]
	[Export] Interactable interactable;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		interactable.InteractionBegin += Begin;
		interactable.InteractionEnd += End;
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

