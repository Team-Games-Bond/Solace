using Godot;
using System;

public partial class SimonSaysManager : Node
{
	[ExportGroup("Settings")]
	[Export] public bool isActive = true;
	[Export] public float timeout = 3;
	[Export] public float flashLength = 1;
	[Export] public Godot.Collections.Array<Godot.Collections.Array<int>> sequences { get; set; }
	
	[ExportGroup("Material Settings")]
	[Export] public Material brightMaterial;
	[Export] public Material dimMaterial;
	
	[ExportGroup("References")]
	[Export] public Godot.Collections.Array<Button> buttons { get; set; }
	[Export] public Godot.Collections.Array<MeshInstance3D> indicators { get; set; } //Change to particle effects or something?

	[Signal] public delegate void SequenceCompletedEventHandler();
	[Signal] public delegate void Sequence1EventHandler();
	[Signal] public delegate void Sequence2EventHandler();
	[Signal] public delegate void Sequence3EventHandler();
	[Signal] public delegate void Sequence4EventHandler();
	[Signal] public delegate void CompletedEventHandler();
	
	//Other variables
	private int currentSequence = 0;
	private int currentNumberInSequence = 0;
	private bool isFlashing = false;

	//timers
	private float idleTimer = 0;
	private float flashTimer = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var indicator in indicators)
		{
			indicator.SetSurfaceOverrideMaterial(0, dimMaterial);
		}

		foreach (var button in buttons)
        {
            button.ButtonPress += ButtonPressed;
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!isActive) return;

		if(isFlashing)
		{
			ResetIdleTimer();
			flashTimer += (float)delta;

			if(flashTimer >= flashLength)
			{
				flashTimer = 0;
				indicators[sequences[currentSequence][currentNumberInSequence]].SetSurfaceOverrideMaterial(0, dimMaterial);

				if(currentNumberInSequence >= sequences[currentSequence].Count-1) endPatternDisplay();
				else currentNumberInSequence += 1;
			}
			else indicators[sequences[currentSequence][currentNumberInSequence]].SetSurfaceOverrideMaterial(0, brightMaterial);
		}

		idleTimer += (float)delta;
		if(idleTimer >= timeout) startPatternDisplay();

	}

	private void ButtonPressed(Button pressedButton)
	{
		//GD.Print("Simon Says Pressed");

		ResetIdleTimer();
		if(isFlashing) endPatternDisplay();

		int pressedButtonIndex = getButtonIndex(pressedButton);

		//GD.Print("Pressed Button: ", pressedButtonIndex);
		//GD.Print("Sequence Number: ", currentSequence,",",currentNumberInSequence);
		//GD.Print("Desired Button: ", sequences[currentSequence][currentNumberInSequence]);
		//GD.Print("");

		//Check if correct
		if(pressedButtonIndex == sequences[currentSequence][currentNumberInSequence])
		{
			//Check if complete
			if(currentNumberInSequence >= sequences[currentSequence].Count-1) sequenceCompleted();
			else currentNumberInSequence += 1;
		}
		else startPatternDisplay();
	}

	private void sequenceCompleted()
	{
		currentNumberInSequence = 0;
		//Check if complete
		signalSequences();
		if(currentSequence >= sequences.Count-1) puzzleCompleted();
		else currentSequence += 1;
		//GD.Print("Sequence Completed");
		startPatternDisplay();
	}

	private void startPatternDisplay()
	{
		currentNumberInSequence = 0;
		isFlashing = true;
		flashTimer = 0;
	}

	private void endPatternDisplay()
	{
		//GD.Print("End Sequence ", currentNumberInSequence);
		indicators[sequences[currentSequence][currentNumberInSequence]].SetSurfaceOverrideMaterial(0, dimMaterial);
		isFlashing = false;
		currentNumberInSequence = 0;
	}

	private void puzzleCompleted()
	{
		GD.Print("Completed!");
        EmitSignal(SignalName.Completed);
	}

	private void ResetIdleTimer()
	{
		idleTimer = 0;
	}

	private int getButtonIndex(Button button)
	{
		for (int i = 0; i < buttons.Count; i++)
		{
			if(button == buttons[i]) return i;
		}
		return -1;
	}

	public void setActive(bool active)
	{
		isActive = active;
	}

	private void signalSequences()
	{
		//GD.Print("Sequence: " + currentSequence);
		if(currentSequence == 0) EmitSignal(SignalName.Sequence1);
		else if(currentSequence == 1) EmitSignal(SignalName.Sequence2);
		else if(currentSequence == 2) EmitSignal(SignalName.Sequence3);
		else if(currentSequence == 3) EmitSignal(SignalName.Sequence4);
	}
}
