using Godot;
using System;
using System.Reflection;

public partial class WindowControl : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && @event.IsPressed()){
			if (keyEvent.Keycode == Key.F11){
				var mode = DisplayServer.WindowGetMode((int)keyEvent.WindowId)==DisplayServer.WindowMode.Windowed?
					DisplayServer.WindowMode.ExclusiveFullscreen: DisplayServer.WindowMode.Windowed;
				DisplayServer.WindowSetMode(mode, (int)keyEvent.WindowId);
			}
		}
    }
}
