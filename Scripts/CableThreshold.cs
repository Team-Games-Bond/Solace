using Godot;
using System;

public partial class CableThreshold : Node
{
	[Export] int RequiredSignals = 2; 
	[Signal]
	public delegate void ThresholdReachedEventHandler();
	public void Power(){
		GD.Print(RequiredSignals);
		if(--RequiredSignals<=0)EmitSignal(SignalName.ThresholdReached);
		GD.Print(RequiredSignals);
	}
}
