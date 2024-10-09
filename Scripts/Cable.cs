using Godot;
using System.Collections.Generic;
using GdDictionary = Godot.Collections.Dictionary;

public partial class Cable : GeometryInstance3D, IPowerable
{
	// This is not an indicator of if the visual transition has completed
	public enum PowerState {
		Inactive,
		Powering,
		Powered
	}
	public PowerState powerState = PowerState.Inactive;
	public double timer=0;
	[Export] public bool start = false;
	[Export] public Material CableAnimationShader = GD.Load<Material>("res://Materials/CableAnimation.tres");
	[Export] public float length = 6f;
	[Export] public GdDictionary Recievers{
		get
		{
			var dict = new GdDictionary();
			foreach (var kvp in _recievers)
			{
				dict.Add(kvp.Key, kvp.Value);
			}
			return dict;
		} 
		set
		{
			_recievers = new SortedList<float, NodePath>();
			foreach (var kvp in value)
			{
				_recievers.Add((float)kvp.Key, (NodePath)kvp.Value);
			}
		} 
	} // ONLY FOR USE FROM EDITOR
	private SortedList<float, NodePath> _recievers = new SortedList<float, NodePath>();
	IEnumerator<KeyValuePair<float, NodePath>> _recieverStream = null;
	[Signal]
	public delegate void CableEndReachedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_recieverStream = _recievers.GetEnumerator();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer += delta;
		SetInstanceShaderParameter("time", timer/length);
		if (powerState == PowerState.Powering){
			if (_recieverStream.Current.Key <= timer){
				GetNode<IPowerable>(_recieverStream.Current.Value).Power();
				if (!_recieverStream.MoveNext()) {
					powerState = PowerState.Powered;
					EmitSignal(SignalName.CableEndReached);
				} 
			}
		}
	}

    public void Power()
    {
		MaterialOverride = CableAnimationShader;
		timer = 0;
		SetInstanceShaderParameter("time_offset", timer);
		if (_recieverStream.MoveNext()){
			powerState = PowerState.Powering;
			//powerTimer.Timeout += ()=>{powerState = PowerState.Powered;};
		} else {
			powerState = PowerState.Powered;
		}
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey){
			if (eventKey.Pressed && eventKey.Keycode==Key.G && start){
				Power();
			}
		}
    }
}
