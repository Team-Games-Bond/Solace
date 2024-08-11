using System;
using System.Collections.Generic;
using Godot;

// Needs tool annotation for warnings to work properly
[Tool]
public partial class ExampleInteraction : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Use for code you don't want running in editor
		if (!Engine.IsEditorHint()){
			// Call _Ready from the base class if you override it
			// Also call _GetConfigurationWarnings if you override it
			Interactable interactable = GetParent<Interactable>();
			interactable.InteractionBegin += Begin;
			interactable.InteractionEnd += End;
		}
	}
    public override void _Process(double delta)
    {
    }

    public void Begin()
    {
		GD.PrintRich("[shake rate=20.0 level=5 connected=1][rainbow]Player Interacted[/rainbow][/shake]");
    }
	public void End()
    {
		
    }
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
}
