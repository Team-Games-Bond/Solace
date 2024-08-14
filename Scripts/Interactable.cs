using Godot;
using System.Collections.Generic;

[Tool]
public partial class Interactable : Area3D
{
	[Signal]
	public delegate void InteractionBeginEventHandler(CharacterController player);
	[Signal]
	public delegate void InteractionEndEventHandler();
	[Export]
	private AnimationPlayer animationPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!Engine.IsEditorHint()){
			animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
			BodyEntered += Enter;
			BodyExited += Exit;
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
	public void Interact(CharacterController player){
		if (canInteract(player)){
			EmitSignal(SignalName.InteractionBegin, player);
		}
	}

	// Prompt player for and enable interaction  
	private void Highlight(){
		animationPlayer.Play("Button Prompt Appear");
	}
	// Hide prompt and disable interaction
	private void Unhighlight(){
		animationPlayer.Play("Button Prompt Disappear");
	}
	public bool canInteract(CharacterController player){
		return true;
	}
    public void Enter(Node3D body)
	{
		CharacterController player = (CharacterController)body;
		if (canInteract(player)){
			if (player.current!=null){
				player.current.Unhighlight();
			}
			Highlight();
			player.current = this;
		}
	}

	public void Exit(Node3D body)
	{
		CharacterController player = (CharacterController)body;
		if (player.current == this){
			Unhighlight();
			player.current = null;
		} 
	}
}
