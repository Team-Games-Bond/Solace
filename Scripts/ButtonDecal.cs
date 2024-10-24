using Godot;
using System;

public partial class ButtonDecal : Decal
{
	[Export] public Key Key;
	[Export] public Texture2D TexturePressed;
	[Export] public Texture2D TextureReleased;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TextureAlbedo = Input.IsKeyPressed(Key)? TexturePressed:TextureReleased;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if ((@event is InputEventKey eventKey) && eventKey.Keycode == Key){
			if(eventKey.IsPressed()){
				TextureAlbedo = TexturePressed;
			}
			else if (eventKey.IsReleased())
			{
				TextureAlbedo = TextureReleased;
			}
		}
	}
}
