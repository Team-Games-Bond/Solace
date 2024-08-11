using Godot;
using System;
using System.ComponentModel;
// This class is a Singleton, you do not need to included it in every Scene
public partial class InWorldCursor : RayCast3D
{
	[Export]
	private float CastLength = 100f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetCollisionMaskValue(3, true);
		SetCollisionMaskValue(1, false);
		CollideWithAreas = true;
		CollideWithBodies = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Camera3D cam = GetViewport().GetCamera3D();
		GlobalPosition = cam.ProjectRayOrigin(GetViewport().GetMousePosition());
		TargetPosition = cam.ProjectRayNormal(GetViewport().GetMousePosition())*CastLength;
	}
	public override void _Input(InputEvent @event){
		if (@event is InputEventMouseButton mouseEvent){
			var hit = GetCollider();
			if (hit != null && hit is InWorldClickable casterHit){
				casterHit.clickEvent(mouseEvent);
			}
		}
	}
}
