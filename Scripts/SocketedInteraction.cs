using Godot;
using System;
using System.Linq;

public partial class SocketedInteraction : Interactable
{
	[Export] public Node3D MountPoint;
	[Export] public Node3D Item;
	private bool isActive = true;
	private bool isLocked = false;
	private string ItemKey = null;

	public bool HasItem()
	{
		return Item != null;
	}

	public bool canInteract(CharacterController player)
	{
		return (HasItem()^player.HasItem()) && isActive;
	}

	public bool itemMatchesKey(Node3D item)
	{
		return item.GetMeta("ItemKey").AsString() == ItemKey;
	}

	public override void Interact(CharacterController player){
		if (canInteract(player))
		{
			//GD.Print("Socket Interacted with");
			
			if(isLocked && !itemMatchesKey(player.Carrying)) return; //Don't allow if it is locked and the item doesn't match.
			//TODO ------ Make the above give off a signal so we can show the player a response.

			(Item, player.Carrying) = (player.Carrying, Item);
			if (Item!=null) {
				player.ItemMount.RemoveChild(Item);
				MountPoint.AddChild(Item);
				
				if(isLocked) LockInItem();
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

	public bool AttachItem(Node3D item)
	{
		if(HasItem()) return false;
		Item = item;
		MountPoint.AddChild(Item);
		return true;
	}

	public void LockInItem()
	{
		GD.Print("Item Locked");
		setActive(false);
		try //to activate code
		{
			var lockedItem = GetNode<LockedItem>(Item.GetPath());
			lockedItem.Activate();
		} catch {
			GD.PushWarning("Locked Item \"", Item.Name, "\" Doesn't Have Script");
		}
	}   

	public void UnlockItem()
	{
		setActive(true);
	}   

	public void SetLockedKey(string key) //Set key to null if you want to unlock
	{
		GD.Print("Set locked key to: ", key);
		ItemKey = key;

		if(ItemKey == null) isLocked = false;
		else isLocked = true;
	}
}
