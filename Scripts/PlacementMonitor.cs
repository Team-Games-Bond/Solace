using Godot;
using System;
using System.Collections.Generic;

public partial class PlacementMonitor : Node3D
{
	[Export] public String ItemKey = "";
	[Export] SocketedInteraction socket;
	[Signal] public delegate void ItemPlacedEventHandler(bool correctItem);
	[Signal] public delegate void ItemRemovedEventHandler();

	public override void _Ready()
    {
        socket.InteractionBegin += Begin;
    }

	public void Begin(CharacterController player)
	{
		if (socket.Item!=null){
			var itemType = socket.Item.GetMeta("ItemKey");
			EmitSignal(SignalName.ItemPlaced, ItemKey=="" || itemType.AsString() == ItemKey);

			//GD.Print("Item Placed. Correct:", ItemKey=="" || itemType.AsString() == ItemKey);
		}
		else {
			EmitSignal(SignalName.ItemRemoved);
		} 
	}
	// Called when interaction ended
	public void End()
	{
		
	}
}

