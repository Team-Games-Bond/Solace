using Godot;
using System;
using System.Collections.Generic;

public partial class PlacementMonitor : Node3D
{
	[ExportGroup("Settings")]
	[Export] public String ItemKey = "";
	[Export] bool SendRaw = false;
	[Export] bool isActive = true;

	[ExportGroup("Setup")]
	[Export] SocketedInteraction socket;

	[Signal] public delegate void ItemPlacedEventHandler(bool correctItem, PlacementMonitor thisMonitor);
	[Signal] public delegate void ItemPlacedRawEventHandler(Node3D item, PlacementMonitor thisMonitor);
	[Signal] public delegate void ItemRemovedEventHandler();
	[Signal] public delegate void ItemRemovedRawEventHandler(PlacementMonitor thisMonitor);

	public override void _Ready()
    {
        socket.InteractionBegin += Begin;
		setSocketActive(isActive);
    }

	public void Begin(CharacterController player)
	{
		if (socket.Item!=null){
			if(!SendRaw)
			{
				var itemType = socket.Item.GetMeta("ItemKey");
				EmitSignal(SignalName.ItemPlaced, ItemKey=="" || itemType.AsString() == ItemKey, this);
			}
			else
			{
				EmitSignal(SignalName.ItemPlacedRaw, socket.Item, this);
			}

			//GD.Print("Item Placed. Correct:", ItemKey=="" || itemType.AsString() == ItemKey);
		}
		else 
		{
			if(!SendRaw) EmitSignal(SignalName.ItemRemoved);
			else EmitSignal(SignalName.ItemRemovedRaw, this);
		} 
	}

	public void setItemKey(String newItemKey)
	{
		ItemKey = newItemKey;
	}

	public void SetLockedKey(string newItemKey) //Set to null to remove lock
	{
		setItemKey(newItemKey);
		socket.SetLockedKey(newItemKey);
	}

	public void setSocketActive(bool active)
	{
		isActive = active;
		socket.setActive(active);
	}

	public bool attachItem(Node3D item) //Returns false if item already attached
	{
		return socket.AttachItem(item);
	}

}

