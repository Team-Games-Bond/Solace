using Godot;
using System;
using System.Collections.Generic;

[Tool]
public abstract partial class AbstractInteraction : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetParent<Interactable>().Interaction += Interact;
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
	// Use to add functionality
	abstract public void Interact();
}
