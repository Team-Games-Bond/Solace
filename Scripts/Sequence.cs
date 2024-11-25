using Godot;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

public partial class Sequence : Node
{
    [Export] public Godot.Collections.Array<Button> sequenceButtons { get; set; }
    [Export] bool reusable = false;
	[Export] bool isEnabled = true;
    [Signal] public delegate void CompletedEventHandler();
    [Signal] public delegate void CorrectEventHandler();
    [Signal] public delegate void IncorrectEventHandler();
    [Signal] public delegate void SequenceUpdatedEventHandler(int index);
    private int sequenceIndex = 0;
	
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (var button in sequenceButtons)
        {
            button.ButtonPress += ButtonPressed;
        }
    }

    public void ButtonPressed(Button button)
    {
		GD.Print("Current Sequence:", sequenceIndex);
        if(button == sequenceButtons[sequenceIndex] && isEnabled)
        {
            SequenceCorrect();
            GD.Print("Correct");
        } else 
        {
            SequenceIncorrect();
            GD.Print("Incorrect");
        }
        EmitSignal(SignalName.SequenceUpdated, sequenceIndex);
    }

    private void SequenceCorrect()
    {
        sequenceIndex += 1;
        if(sequenceIndex >= sequenceButtons.Count) SequenceCompleted();
        else EmitSignal(SignalName.Correct);
    }

    private void SequenceIncorrect()
    {
        sequenceIndex = 0;
        EmitSignal(SignalName.Incorrect);
    }

    private void SequenceCompleted()
    {
        GD.Print("Completed!");
        EmitSignal(SignalName.Completed);
        if(reusable) sequenceIndex = 0;
		else isEnabled = false;
    }
}