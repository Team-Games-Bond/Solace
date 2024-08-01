using Godot;
using System.Collections.Generic;

[Tool]
public partial class Interactable : Area3D
{
	/*
	 * Interaction System
	 * By Maddie
	 */

	[Export]
	private string InteractButton = "Interact";
	private AnimationPlayer animationPlayer;
	private static Interactable current = null;
	private bool inRange = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!Engine.IsEditorHint()){
			animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
			BodyEntered += enter;
			BodyExited += exit;
		}
	}
    public override string[] _GetConfigurationWarnings()
    {
		List<string> warnings = new List<string>();
		if (GetNodeOrNull<AnimationPlayer>("AnimationPlayer") == null){
			warnings.Add("This node has no AnimationPlayer, it is required for Interactable to work");
		}
        return warnings.ToArray();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (Engine.IsEditorHint()){
			UpdateConfigurationWarnings();
		}
	}
	public void Interact(){
		GD.PrintRich("[shake rate=20.0 level=5 connected=1][rainbow]Player Interacted[/rainbow][/shake]");
	}
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(InteractButton)&inRange){
			Interact();
		}
    }
	private void Highlight(){
		if(current!=null){
			Unhighlight();
		}
		current = this;
		GD.PrintRich("[b]Body has entered interactable[/b]");
		animationPlayer.Play("Button Prompt Appear");
		inRange = true;
	}
	private void Unhighlight(){
		GD.PrintRich("[b]Body has exited interactable[/b]");
		animationPlayer.Play("Button Prompt Disappear");
		inRange = false;
		current = null;
	}
	
    private void Enter(Node3D body)
	{
		Highlight();
	}

	private void Exit(Node3D body)
	{
		Unhighlight();
	}
}
