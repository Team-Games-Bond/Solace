using Godot;
using System;

public partial class NeuronActivator : Node
{
	[Export] public Fog fog;

	[Export] public Godot.Collections.Array<Texture2D> textures {get; set; }

	int i = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("DebugClick")){
			GD.Print("Applying Fog ",i);
			fog.Recede(textures[i]);
			i++;
		}
    }
}
