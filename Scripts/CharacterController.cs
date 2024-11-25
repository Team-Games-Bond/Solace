using Godot;
using System;

public partial class CharacterController : CharacterBody3D
{
	[ExportGroup("Controller Settings")]
	[Export] public float Speed = 8.0f;
	[Export] public Vector3 Drag = new Vector3(3.0f, 0.5f, 3.0f);
	[Export] public float JumpVelocity = 4.5f;
	[Export] public float RotationSpeed = 0.1f;

	[ExportGroup("Controller Setup")]
	[Export] public Node3D PlayerPivot;
	[Export] public Area3D LadderDetector; //If you remove this the turning system breaks... somehow?????
	private AudioStreamPlayer sfx_push;

	//Other variables
	public bool wasOnFloorLastFrame = false; 
	public bool wasJustTeleported = false;
	public bool isPuzzleMode = false;
	public Interactable Current;
	public Godot.Collections.Array<Interactable> CloseInteractables;
	Vector3 walkVelocity;

	[ExportGroup("Item Carrying")]
	[Export] public Node3D ItemMount;
	public Node3D Carrying;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public Vector3 gravity {get=>GetGravity();}//= ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		CloseInteractables = new Godot.Collections.Array<Interactable>();
		sfx_push = GetNode<AudioStreamPlayer>("push");
	}

	public override void _PhysicsProcess(double delta)
	{
		//Don't do any of this if we are in puzzle mode
		if(isPuzzleMode) return;

		//Get Current Velocity
		Vector3 velocity = Velocity;

		// Add the gravity.
		velocity += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("CharLeft", "CharRight", "CharUp", "CharDown");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			TurnCharacter(inputDir);

			//Move character
			velocity.X += direction.X * Speed * (float)delta;
			velocity.Z += direction.Z * Speed * (float)delta;

		}
		/*else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}*/
		Velocity = velocity*(Vector3.One-(float)delta*Drag);
		MoveAndSlide();

		wasOnFloorLastFrame = IsOnFloor();
	}

	public override void _Process(double delta)
	{
		if(CloseInteractables.Count <= 0) return; //Don't do this if there is nothing nearby

		float cDist = 10000;
		Interactable closest = null;
		foreach (var interactable in CloseInteractables)
		{
			var dist = this.GlobalPosition.DistanceTo(interactable.GlobalPosition);
			if(dist < cDist)
			{
				closest = interactable;
				cDist = dist;
			}
		}
		if (!(Current == closest))
		{
			if(Current != null) Current.Unhighlight();
			Current = closest;
			Current.Highlight();
		}
	}

	void TurnCharacter(Vector2 inputDir)
	{
		//Turning character
			var current = new Vector3(-PlayerPivot.Basis.Z.X,0,-PlayerPivot.Basis.Z.Z).Normalized();
			var goal = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
			var lookDirection = current.Slerp(goal, RotationSpeed);

			PlayerPivot.Basis = Basis.LookingAt(lookDirection);
			
			//Floating point error prevention
			//PlayerPivot.Transform = PlayerPivot.Transform.Orthonormalized();
	}
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Interact") && Current!=null){
			Current.Interact(this);
			sfx_push.Play();
		}
	}

	public bool HasItem(){
		return Carrying != null;
	}

	public void Teleport(Vector3 pos)
	{
		this.Position = pos;
		GD.Print(this.Position," vs target: ", pos);
	}
}
