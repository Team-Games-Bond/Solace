using Godot;
using System;

[Tool]
public partial class Button : Node3D
{
	[Signal]
	public delegate void ButtonPressEventHandler(Button button);

	[ExportGroup("Internal Setup")]
	[Export] Interactable interactable;
	[Export] bool isActive = true;

	// Bounce settings
	[Export] bool bounceEffect = true; // Toggle bounce effect
	[Export] float verticalBounceAmount = 0.2f; // Vertical bounce distance
	[Export] float horizontalBounceAmount = 0.1f; // Horizontal bounce (inwards) distance
	[Export] float bounceDuration = 0.2f; // Time before returning to original position
	[Export] bool verticalBounce = true; // If true, bounces vertically. If false, bounces horizontally (inwards)

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		interactable.InteractionBegin += Begin;
	}

	// Called when the button is pressed
	public void Begin(CharacterController player)
	{
		if (isActive)
		{
			EmitSignal(SignalName.ButtonPress, this);

			if (bounceEffect)
			{
				// Determine bounce direction and amount
				Vector3 bounceVector = verticalBounce 
					? Vector3.Down * verticalBounceAmount 
					: Vector3.Forward * horizontalBounceAmount;

				TranslateObjectLocal(bounceVector); // Apply bounce

				// Return to original position after bounceDuration
				GetTree().CreateTimer(bounceDuration).Timeout += () => TranslateObjectLocal(-bounceVector); // Return to original position
			}
		}
	}

	// Set the button to be active or inactive
	public void SetActive(bool active)
	{
		isActive = active;
		interactable.SetActive(active);
	}
}
