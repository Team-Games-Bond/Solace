using Godot;
using System.Collections.Generic;

[Tool]
public partial class Interactable : Area3D
{
	[Signal]
	public delegate void InteractionBeginEventHandler();
	[Signal]
	public delegate void InteractionEndEventHandler();
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
			BodyEntered += Enter;
			BodyExited += Exit;
			Unhighlight();
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
		EmitSignal(SignalName.InteractionBegin);
	}
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(InteractButton)&inRange){
			Interact();
		}
    }

	// Prompt player for and enable interaction  
	private void Highlight(){
		if(current!=null){
			Unhighlight();
		}
		current = this;
		animationPlayer.Play("Button Prompt Appear");
		inRange = true;
	}
	// Hide prompt and disable interaction
	private void Unhighlight(){
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
