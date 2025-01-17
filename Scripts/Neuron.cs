using Godot;
using System;

public partial class Neuron : Node
{
	[Export] public Cable cable;
	[Export] public bool startOn = false;

	[ExportGroup("Material Settings")]
	[Export] public Material brightMaterial;
	[Export] public Material dimMaterial;
	[Export] public float lightEnergy = 3;

	[ExportGroup("references")]
	[Export] public MeshInstance3D flagella;
	[Export] public MeshInstance3D sphere;
	[Export] public Light3D light;


	[Signal] public delegate void NeuronActivatedEventHandler();
	[Signal] public delegate void NeuronActivatedWithReferenceEventHandler(Neuron neuron);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		light.LightEnergy = lightEnergy;
		if(startOn) SetMaterial(brightMaterial);
		else SetMaterial(dimMaterial);

		if(cable != null) cable.CableEndReached += PowerNeuron;
	}

	public void PowerNeuron()
	{
		SetMaterial(brightMaterial);
		EmitSignal(SignalName.NeuronActivated);
		EmitSignal(SignalName.NeuronActivatedWithReference, this);
	}

	public void SetMaterial(Material material)
	{
		flagella.SetSurfaceOverrideMaterial(0, material);
		sphere.SetSurfaceOverrideMaterial(0, material);
		#if !(GODOT_MOBILE||GODOT_WEB)
		if(material == dimMaterial)
		{
			light.Visible = false;
		} else if (material == brightMaterial)
		{
			light.Visible = true;
		}
		#else
		light.Visible = false;
		#endif
	}
}
