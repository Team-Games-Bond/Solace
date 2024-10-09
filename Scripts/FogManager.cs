using Godot;
using System;

public enum FogLocation
{
	maze1,
	maze2,
	clutterAndSimon,
	bridge,
	tree
}

public partial class FogManager : Node
{
	[Export] public Fog fog;

	[Export] public Godot.Collections.Array<Texture2D> textures {get; set; }[Signal]
	public delegate void FogRemovedEventHandler(FogLocation fogLocation);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RemoveFog(FogLocation fogloc)
	{
		EmitSignal(SignalName.FogRemoved, (int)fogloc);
		switch (fogloc)
		{
			case FogLocation.maze1:
				fog.Recede(textures[0]);
				return;
			case FogLocation.maze2:
				fog.Recede(textures[1]);
				return;
			case FogLocation.clutterAndSimon:
				fog.Recede(textures[2]);
				return;
			case FogLocation.bridge:
				fog.Recede(textures[3]);
				return;
			case FogLocation.tree:
				fog.Recede(textures[4]);
				return;
			default:
				GD.PushError("Unhandled Fog Location: " + fog.ToString());
				return;
		}
	}

}
