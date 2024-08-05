using Godot;

// Needs tool annotation for warnings to work properly
[Tool]
public partial class ExampleInteraction : AbstractInteraction
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Use for code you don't want running in editor
		if (!Engine.IsEditorHint()){
			// Call _Ready from the base class if you override it
			// Also call _GetConfigurationWarnings if you override it
			base._Ready();
		}
	}
    public override void _Process(double delta)
    {
    }

    public override void Interact()
    {
		GD.PrintRich("[shake rate=20.0 level=5 connected=1][rainbow]Player Interacted[/rainbow][/shake]");
    }
}
