using Godot;
using System;

public partial class SocketedInteraction : Interactable
{
    [Export] public Node3D MountPoint;
    [Export] public Node3D Item;
    private bool isActive = true;
    public bool HasItem()
    {
        return Item != null;
    }
    public bool canInteract(CharacterController player)
    {
		return HasItem()^player.HasItem();
	}
    public override void Interact(CharacterController player){
        if (canInteract(player) && isActive)
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

    public void setActive(bool active)
    {
        isActive = active;
    }

    public bool attachItem(Node3D item)
    {
        if(HasItem()) return false;
        Item = item;
        MountPoint.AddChild(Item);
        return true;
    }
}
