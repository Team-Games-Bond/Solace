using Godot;
using System;

public partial class FootprintFade : Decal
{
	SceneTreeTimer timer;
	[Export] public float FadeStart = 3;
	[Export] public float FadeDuration = 1;
	float alphaStart;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		alphaStart = AlbedoMix;
		GetTree().CreateTimer(FadeStart).Timeout+=()=>{
			timer = GetTree().CreateTimer(FadeDuration);
			timer.Timeout+=()=>{GetParent().Dispose();};
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(timer is not null){
			AlbedoMix = alphaStart*((float)timer.TimeLeft/FadeDuration);
		}
	}

}
