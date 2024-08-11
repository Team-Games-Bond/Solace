using Godot;
using System;

public partial class InWorldClickable : Node
{
	[Signal]
	public delegate void InWorldLeftClickEventHandler();
	[Signal]
	public delegate void InWorldMouseButtonEventHandler(InputEventMouseButton mouseEvent);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void clickEvent(InputEventMouseButton mouseEvent){
		if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left){
			EmitSignal(SignalName.InWorldLeftClick);
		}
		EmitSignal(SignalName.InWorldMouseButton, mouseEvent);
	}
}
