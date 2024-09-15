using Godot;
using System;

public partial class TeleportDoor : Area3D
{
	[Export] public Node3D destination;
	// Called when the node enters the scene tree for the first time.
	private bool justActivated = false;
	public override void _Ready()
	{
		BodyEntered += Enter;
		BodyExited += Exit;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void Enter(Node3D body)
	{
		CharacterController player = null;
		try {player = (CharacterController)body; } 
		catch (Exception e) 
		{
			GD.PushWarning(e);
			return;
		}

		if(player.wasJustTeleported) return;
		
		player.wasJustTeleported = true;
		justActivated = true;

		player.Teleport(destination.Position);
	}
	private void Exit(Node3D body)
	{
		CharacterController player = (CharacterController)body;
		if(!justActivated) player.wasJustTeleported = false;
		else justActivated = false;
	}
}
