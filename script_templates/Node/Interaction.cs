// meta-description: Template for node that recieves interaction events from it's parent

using _BINDINGS_NAMESPACE_;
using System;
using System.Collections.Generic;


[Tool]
public partial class _CLASS_ : _BASE_
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Use for code you don't want running in editor
		if (!Engine.IsEditorHint()){
			// Call _Ready from the base class if you override it
			// Also call _GetConfigurationWarnings if you override it
			GetParent<Interactable>().InteractionBegin += Begin;
			GetParent<Interactable>().InteractionEnd += End
		}
	}
    public override void _Process(double delta)
    {
    }
	// Called when parent interactable is interacted with
    public void Begin(CharacterController player)
    {
		
    }
	// Called when interaction ended
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
