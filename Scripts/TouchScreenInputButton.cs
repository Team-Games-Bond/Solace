using Godot;
using System;

public partial class TouchScreenInputButton : BaseButton
{
	[Export]
	public string Action;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ButtonUp += Release;
		ButtonDown += Press;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public void Release(){
		var action = new InputEventAction();
		action.Action = Action;
		action.Pressed = false;
		Input.ParseInputEvent(action);
	}
	public void Press(){
		var action = new InputEventAction();
		action.Action = Action;
		action.Pressed = true;
		Input.ParseInputEvent(action);
	}
}
