using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;
using JTTF.Management;
using MLAPI.Messaging;

#pragma warning disable IDE0044

namespace JTTF.Inventory
{
    public class InventoryController : CustomNetworkBehaviour
    {
        [SerializeField] private Inventory inventory = null;

		protected virtual int InventorySize => 0;

		public Inventory Inventory => inventory;
		public Action OnInventoryChange { get => inventory.onInventoryChange; set => inventory.onInventoryChange = value; }

		protected override void Awake()
		{
			base.Awake();

			inventory.Init(InventorySize);

			InventoryManager.AddNewInventory(inventory);
		}

		[ServerRpc(RequireOwnership = false)]
		private void AddItemAtServerRpc(int displayIndex, int amount)
		{
			inventory.AddItemAt(displayIndex, amount);
		}
		[ServerRpc(RequireOwnership = false)]
		private void RemoveItemAtServerRpc(int displayIndex, int amount)
		{
			inventory.RemoveItemAt(displayIndex, amount);
		}

		[ServerRpc(RequireOwnership = false)]
		private void AddItemServerRpc(Item item, int amount)
		{
			inventory.AddItem(item, amount);
		}
		[ServerRpc(RequireOwnership = false)]
		private void RemoveItemServerRpc(Item item, int amount)
		{
			inventory.RemoveItem(item, amount);
		}

		[ServerRpc(RequireOwnership = false)]
		private void MoveItemServerRpc(int from, int to)
		{
			inventory.MoveItem(from, to);
		}
		[ServerRpc(RequireOwnership = false)]
		private void MoveItemServerRpc(int from, ulong otherInventoryObjectID, ulong otherInventoryBehaviourID, int to)
		{
			var otherInventory = InventoryManager.GetInventory(otherInventoryObjectID, otherInventoryBehaviourID);
			if (otherInventory != null)
			{
				inventory.MoveItem(from, otherInventory, to);
			}
		}

		[ServerRpc(RequireOwnership = false)]
		private void SwapItemServerRpc(int from, int to)
		{
			inventory.SwapItem(from, to);
		}
		[ServerRpc(RequireOwnership = false)]
		private void SwapItemServerRpc(int from, ulong otherInventoryObjectID, ulong otherInventoryBehaviourID, int to)
		{
			var otherInventory = InventoryManager.GetInventory(otherInventoryObjectID, otherInventoryBehaviourID);
			if (otherInventory != null)
			{
				inventory.SwapItem(from, otherInventory, to);
			}
		}

		public bool CanAddItem(Item item, int amount)
		{
			return inventory.CanAddItem(item, amount);
		}

		public void AddItemAt(int displayIndex, int amount)
		{
			if (GameManager.IsMulti)
				AddItemAtServerRpc(displayIndex, amount);
			else
				inventory.AddItemAt(displayIndex, amount);
		}
		public void RemoveItemAt(int displayIndex, int amount)
		{
			if (GameManager.IsMulti)
				RemoveItemAtServerRpc(displayIndex, amount);
			else
				inventory.RemoveItemAt(displayIndex, amount);
		}

		public void AddItem(Item item, int amount)
		{
			if (GameManager.IsMulti)
				AddItemServerRpc(item, amount);
			else
				inventory.AddItem(item, amount);
		}
		public void RemoveItem(Item item, int amount)
		{
			if (GameManager.IsMulti)
				RemoveItemServerRpc(item, amount);
			else
				inventory.RemoveItem(item, amount);
		}

		public void MoveItem(int from, int to)
		{
			if (GameManager.IsMulti)
				MoveItemServerRpc(from, to);
			else
				inventory.MoveItem(from, to);
		}
		public void MoveItem(int from, Inventory otherInventory, int to)
		{
			if (GameManager.IsMulti)
				MoveItemServerRpc(from, otherInventory.NetworkObjectId, otherInventory.NetworkBehaviourId, to);
			else
				inventory.MoveItem(from, otherInventory, to);
		}

		public void SwapItem(int from, int to)
		{
			if (GameManager.IsMulti)
				SwapItemServerRpc(from, to);
			else
				inventory.SwapItem(from, to);
		}
		public void SwapItem(int from, Inventory otherInventory, int to)
		{
			if (GameManager.IsMulti)
				SwapItemServerRpc(from, otherInventory.NetworkObjectId, otherInventory.NetworkBehaviourId, to);
			else
				inventory.SwapItem(from, otherInventory, to);
		}
	}
}
