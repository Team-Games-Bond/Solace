using Godot;
using System;
using System.Collections.Generic;

// Needs tool annotation for warnings to work properly
[Tool]
public partial class CameraInteraction : Camera3D
{
	private CameraController controller;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!Engine.IsEditorHint()){
			// Makes the assumption that there is only one camera controller in a scene
			controller = (CameraController)GetTree().GetFirstNodeInGroup("Character Controllers");
			Interactable interactable = GetParent<Interactable>();
			interactable.InteractionBegin += Begin;
			interactable.InteractionEnd += End;
		}
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
    public override void _Process(double delta)
    {
    }

    public void Begin(CharacterController player)
    {
		controller.ChangeCamera(this);
    }
	public void End()
	{
		controller.ChangeCamera();
	}
}
