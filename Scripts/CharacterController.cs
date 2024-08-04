using Godot;
using System;

public partial class CharacterController : CharacterBody3D
{
	[ExportGroup("Controller Settings")]
	[Export] public float Speed = 5.0f;
	[Export] public float JumpVelocity = 4.5f;
	[Export] public float RotationSpeed = 0.1f;

	[ExportGroup("Controller Setup")]
	[Export] public Node3D PlayerPivot;
	[Export] public Area3D LadderDetector;

	//Other variables
	public bool wasOnFloorLastFrame = false; 
	public bool isPuzzleMode = false;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		//Don't do any of this if we are in puzzle mode
		if(isPuzzleMode) return;

		//Get Current Velocity
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			TurnCharacter(inputDir);

			//Move character
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;

		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		wasOnFloorLastFrame = IsOnFloor();
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

}