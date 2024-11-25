using Godot;
using System;

public partial class SequenceIndicator : MeshInstance3D
{
	[Export] int ActivationStage;
	[Export] Material InertMaterial;
	[Export] Material ActivatedMaterial;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MaterialOverride = InertMaterial;
	}
	public void SequenceUpdated(int SequenceStage){
		if(SequenceStage>=ActivationStage){
			MaterialOverride = ActivatedMaterial;
		} else {
			MaterialOverride = InertMaterial;
		}
	}
}
