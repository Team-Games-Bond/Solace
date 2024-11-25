using Godot;

public partial class DrawType : OptionButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ItemSelected += Swap;
	}

    private void Swap(long index)
    {
		GetTree().Root.DebugDraw = (Viewport.DebugDrawEnum)GetItemId((int)index);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
