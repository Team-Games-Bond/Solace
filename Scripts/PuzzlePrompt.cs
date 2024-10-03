using Godot;
using System;
using System.Collections.Generic;


[Tool]
public partial class PuzzlePrompt : CanvasLayer
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Use for code you don't want running in editor
		if (!Engine.IsEditorHint()){
			// Call _Ready from the base class if you override it
			// Also call _GetConfigurationWarnings if you override it
			GetParent<Interactable>().InteractionBegin += Begin;
		}
	}
	public override void _Process(double delta)
	{
	}
	// Called when parent interactable is interacted with
	public void Begin(CharacterController player)
	{
		Visible = true;
	}
	// Called when interaction ended
	public override string[] _GetConfigurationWarnings()
	{
		List<string> warnings = new List<string>();
		try{
			if (GetParent<Interactable>() == null){
				warnings.Add(string.Format("{0} needs to be a child of an Area3D with an Interactable Script Attatched", GetType().Name ));
			}
		} catch (InvalidCastException)
		{
			warnings.Add(string.Format("{0} needs to be a child of an Area3D with an Interactable Script Attatched", GetType().Name ));
		}
		return warnings.ToArray();
	}
	public void Quit(){
		Visible = false;
	}
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("quick_exit")){
			Quit();
		}
    }
}

