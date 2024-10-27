using Godot;
using System;

public partial class LinkTo : Node
{
	[Signal] public delegate void TriggerLinkEventHandler();

	public void sendSignal()
	{
		GD.Print("EmittingSignal");
		EmitSignal(SignalName.TriggerLink);
	}
}
