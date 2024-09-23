using Godot;
using System.Collections.Generic;

[Tool]
public partial class Interactable : Area3D
{
	[Export] public bool isActive = true;

	[Signal]
	public delegate void InteractionBeginEventHandler(CharacterController player);
	[Signal]
	public delegate void InteractionEndEventHandler();
	[Signal]
	public delegate void PromptShowEventHandler();
	[Signal]
	public delegate void PromptHideEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += Enter;
		BodyExited += Exit;
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			UpdateConfigurationWarnings();
		}
	}

	public virtual void Interact(CharacterController player){
		EmitSignal(SignalName.InteractionBegin, player);
	}

	// Prompt player for and enable interaction  
	public void Highlight(){
		EmitSignal(SignalName.PromptShow);
	}
	// Hide prompt and disable interaction
	public void Unhighlight(){
		EmitSignal(SignalName.PromptHide);
	}
    public virtual void Enter(Node3D body)
	{
		if(!isActive) return;

		CharacterController player = (CharacterController)body;
		if (player.Current!=null){
			player.Current.Unhighlight();
		}
		Highlight();
		player.Current = this;
	}

	public virtual void Exit(Node3D body)
	{
		CharacterController player = (CharacterController)body;
		if (player.Current == this){
			Unhighlight();
			player.Current = null;
		} 
	}

	public void SetActive(bool active)
    {
        isActive = active;
    }
}
