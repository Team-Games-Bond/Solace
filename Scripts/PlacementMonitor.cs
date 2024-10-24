using Godot;
using System;
using System.Collections.Generic;

public partial class PlacementMonitor : Node3D
{
	[Export] public Node3D StartingItem;

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
	[Signal] public delegate void PowerCableEventHandler();

	public override void _Ready()
	{
		socket.InteractionBegin += Begin;
		setSocketActive(isActive);

		//Check if has item pre-placed
		if(StartingItem != null) attachItem(StartingItem);
	}

	public void Begin(CharacterController player)
	{
		if (socket.Item!=null){
			var itemType = socket.Item.GetMeta("ItemKey");

			if(ItemKey=="" || itemType.AsString() == ItemKey) EmitSignal(SignalName.PowerCable);

			if(!SendRaw)
			{
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
		GD.Print("Attaching ", item.Name);
		var successful = socket.AttachItem(item);
		if(successful) 
		{
			if(!SendRaw)
			{
				var itemType = socket.Item.GetMeta("ItemKey");
				EmitSignal(SignalName.ItemPlaced, ItemKey=="" || itemType.AsString() == ItemKey, this);
			}
			else
			{
				EmitSignal(SignalName.ItemPlacedRaw, socket.Item, this);
			}
		}
		if(successful) GD.Print("Attaching ", item.Name, " was successful");
		return successful;
	}

}
