using Godot;
using System;

public partial class TextDisplay : Node
{
	[Export] public string textToDisplay;
	[Export] public double timeToDisplay = 3;
	[Export] public double timeToType = 0.5;
	[Export] public string rgb = "000";
	[ExportGroup("Setup Settings")]
	[Export] public bool isDwane = false;
	[Export] public Area3D area;
	[Export] public Button button;
	[Export] public CanvasLayer canvas;
	[Export] public RichTextLabel textLabel;
	[Export] public Timer disapearTimer;
	[Export] public Timer typingTimer;

	[Export] public bool isActive = true;

	[Signal]
	public delegate void InteractionBeginEventHandler(CharacterController player);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		textLabel.Text = "[center][color=#"+rgb+"]"+textToDisplay+"[/color][/center]";

		canvas.Visible = false;
		disapearTimer.WaitTime = timeToDisplay;
		typingTimer.WaitTime = timeToType;

		disapearTimer.Timeout += RemoveText;
		if(!isDwane) area.BodyEntered += EnteredZone;
		else if(isDwane) button.ButtonPress += dwaneSpeaks;
	}

    public override void _Process(double delta)
    {
        if (!typingTimer.IsStopped())
		{
			textLabel.VisibleRatio = (float)(1-(typingTimer.TimeLeft/typingTimer.WaitTime));
		} else textLabel.VisibleRatio = 1;
    }

    public void EnteredZone(Node3D body)
	{
		if(!isActive) return;

		CharacterController player = (CharacterController)body;

		StartText();
	}

	public void dwaneSpeaks(Button button)
	{
		if(!isActive) return;
		
		StartText();
	}

	public void StartText()
	{
		canvas.Visible = true;
		disapearTimer.Start();
		typingTimer.Start();
	}

	public void RemoveText()
	{
		if(!isActive) return;

		canvas.Visible = false;
		disapearTimer.Stop();
	}
}
