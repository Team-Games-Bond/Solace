using Godot;
using System;

public partial class InWorldButtonAppearance : GeometryInstance3D
{
	[Export] public CollisionObject3D Button;
	[Export] public Material HoverMaterial;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button.MouseEntered += Highlight;
		Button.MouseExited += Unhighlight;
	}
	public void Highlight()
	{
		GD.Print("Button");
		MaterialOverride = HoverMaterial;
	}
	public void Unhighlight()
	{
		MaterialOverride = null;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
