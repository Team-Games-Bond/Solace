using Godot;
using System;

public partial class ButtonPromptAnimator : Sprite3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void Appear(){
		Visible=true;
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "scale", Vector3.One, 0.1f);
	}
	public void Disappear(){
		var tween = GetTree().CreateTween();
		tween.TweenProperty(this, "scale", Vector3.Zero, 0.1f);
		tween.TweenProperty(this, "visible", false, 0);
	}
}
