using Godot;
using System;

public partial class ColourBoxSpawner : Node
{
	[ExportGroup("Settings")]
	[Export] bool isActive = true;

	[ExportGroup("Setup")]
	[Export] SocketedInteraction socket;

	[Signal] public delegate void ItemRemovedEventHandler();

	private Material spawnMaterial;

	public override void _Ready()
    {
        socket.InteractionBegin += Begin;
		setSocketActive(isActive);
    }

	public void Begin(CharacterController player)
	{
		EmitSignal(SignalName.ItemRemoved);
		if(!spawnItem()) GD.PushError(GetPath(), " Tried to spawn item before item was removed");
	}

	public bool spawnItem() //Returns false if item already exists in socket
	{
        //TODO use model instead of box
        var item = new CsgBox3D
        {
            Material = spawnMaterial
        };
		
        return socket.attachItem(item);

		//TODO Make garbage disposal socket (using .QueueFree()) 
		//Maybe just add metadata to the spawned item and give the socket the ability to dispose of spawned items
	}

	public void setSocketActive(bool active)
	{
		isActive = active;
		socket.setActive(active);
	}
	
	public void setMaterial(Material material)
	{
		spawnMaterial = material;
	}
}
