using Godot;
using System;

public partial class MoveBoxUp : CsgBox3D
{
	[Export] public Sequence sequence;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sequence.Completed += MoveUp;
	}

	
	private void MoveUp()
	{
		this.Position = new Vector3(this.Position.X, this.Position.Y+2, this.Position.Z);
	}
}
