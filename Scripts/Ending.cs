using Godot;
using System;

public partial class Ending : Node
{
	[Export] public string AfterScenePath = "res://Scenes/MainMenu.tscn";
	[Export] public float zoomDistance = 100;
	[Export] public double fadeTime = 5;
	[Export] public double blurStrength = 2;
	[Export] public float scrollSpeed = 1;
	[Export] public double timeBeforeScroll = 8;

	[ExportGroup("Setup Settings")]
	[Export] public Area3D area;
	[Export] public CanvasLayer canvas;
	[Export] public CanvasItem blur;
	[Export] public ColorRect darken;
	[Export] public Control endScreen;
	[Export] public Control EndPoint;
	[Export] public Timer fadeTimer;
	[Export] public Timer scrollTimer;


	ShaderMaterial blurMat;
	private CameraController controller;
	bool isScrolling = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Makes the assumption that there is only one camera controller in a scene
		controller = (CameraController)GetTree().GetFirstNodeInGroup("Character Controllers");

		canvas.Visible = false;
		fadeTimer.WaitTime = fadeTime;
		scrollTimer.WaitTime = timeBeforeScroll;

		scrollTimer.Timeout += StartScroll;
		area.BodyEntered += EnteredZone;

		blurMat = (ShaderMaterial)blur.Material;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!fadeTimer.IsStopped())
		{
			blurMat.SetShaderParameter("blur_strength", (1-fadeTimer.TimeLeft/fadeTimer.WaitTime)*blurStrength);
			darken.Color = new Color(0,0,0,(float)(1-fadeTimer.TimeLeft/fadeTimer.WaitTime));
		} else 
		{
			blurMat.SetShaderParameter("blur_strength", 1*blurStrength);
			darken.Color = new Color(0,0,0,1);
		}

		if(isScrolling)
		{
			endScreen.Position = new Vector2(endScreen.Position.X, endScreen.Position.Y-scrollSpeed);
			if(EndPoint.GlobalPosition.Y <= 0)
			{
				GD.Print("Return to main menu!");
				var MainMenu = ((PackedScene)ResourceLoader.LoadThreadedGet(AfterScenePath)).Instantiate();
				GetTree().Root.AddChild(MainMenu);
				GetNode("/root/Game").QueueFree();
			}
		} 

	}

	public void EnteredZone(Node3D body)
	{
		CharacterController player = (CharacterController)body;

		canvas.Visible = true;
		fadeTimer.Start();
		scrollTimer.Start();
		controller.ZoomCamera(zoomDistance);
	}

	public void StartScroll()
	{
		if (ResourceLoader.LoadThreadedGetStatus(AfterScenePath)==ResourceLoader.ThreadLoadStatus.InvalidResource){
			ResourceLoader.LoadThreadedRequest(AfterScenePath);
		}
		isScrolling = true;
		GD.Print("StartingScroll");
	}
}
