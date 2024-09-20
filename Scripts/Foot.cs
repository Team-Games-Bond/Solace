using Godot;
using Godot.NativeInterop;
using System;

public partial class Foot : Node3D
{
	[Export] public float StepSpeed = 15;
	[Export] public PackedScene footprint = ResourceLoader.Load<PackedScene>("res://Scenes/FootPrint.tscn");
	public Vector3 targetPos;
	Vector3 startPos;

	private double _t = 0;
	public bool moving = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TopLevel=true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(moving){
			var d = startPos.DistanceTo(targetPos);
			_t += delta*StepSpeed;
			if (_t>=1){
				_t = 1;
				moving = false;
			}
			GlobalPosition = MadsHermite(startPos, targetPos, Vector3.Up*d, Vector3.Up*d, (float)_t);
		}
	}
	// Custom NON STANDARD hermite curve where the second normal is INVERSE
	Vector3 MadsHermite(Vector3 p1, Vector3 p2, Vector3 n1, Vector3 n2, float t) {
		return p1*((1+2*t)*(1-t)*(1-t))+n1*(t*(1-t)*(1-t))+p2*(t*t*(3-(2*t)))+n2*(t*t*(1-t));
	}
	
	public void Step(Vector3 target){
		if (!moving){
			targetPos = target;
			moving = true;
			_t = 0;
			startPos = GlobalPosition;
			
			var scene = footprint.Instantiate();
			AddChild(scene);
			((Node3D)scene).TopLevel = true;
			((Node3D)scene).Scale = Vector3.One;
		}
	}
	
}
