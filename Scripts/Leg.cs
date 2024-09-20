using Godot;
using System;

public partial class Leg : RayCast3D
{
	[Export] public Foot foot;
	public bool Grounded{get{return !foot.moving;}}
	[Export] public Leg[] AdjacentLegs;
	bool restFrame=false;
	[Export]double CooldownLength = 0.2;
	double Cooldown;
	static bool pause = false;
	[Export] public float MovementThreshold{
		get{
			return Mathf.Sqrt(_movementThresholdSquared);
		}
		set{
			_movementThresholdSquared = value*value;
		}
	}
	private float _movementThresholdSquared=1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Cooldown = CooldownLength;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Cooldown-=delta;
		if (new Vector2(GlobalPosition.X, GlobalPosition.Z).DistanceSquaredTo(new Vector2(foot.GlobalPosition.X, foot.GlobalPosition.Z))>_movementThresholdSquared){
			if(CanMove){
				foot.Step(GetCollisionPoint());
				Cooldown = CooldownLength;
			}
		}
	}
	bool CanMove{
		get{
			if (foot.moving) return false;
			if (Cooldown>0) return false;
			foreach (Leg leg in AdjacentLegs)
			{
				if (!leg.Grounded) return false;
			}
			return IsColliding();
		}
	}
}
