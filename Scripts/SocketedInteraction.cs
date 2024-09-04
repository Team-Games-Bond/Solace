using Godot;
using System;

public partial class SocketedInteraction : Interactable
{
    [Export] public Node3D MountPoint;
    [Export] public Node3D Item;
    public bool HasItem()
    {
        return Item != null;
    }
    public bool canInteract(CharacterController player)
    {
		return HasItem()^player.HasItem();
	}
    public override void Interact(CharacterController player){
        if (canInteract(player))
        {
            (Item, player.Carrying) = (player.Carrying, Item);
            if (Item!=null) {
                player.ItemMount.RemoveChild(Item);
                MountPoint.AddChild(Item);
            } else {
                MountPoint.RemoveChild(player.Carrying);
                player.ItemMount.AddChild(player.Carrying);
            }
            base.Interact(player);
        }
    }
    public override void Enter(Node3D body)
    {
        CharacterController player = (CharacterController)body;
        if (canInteract(player)) base.Enter(body);
    }
}
