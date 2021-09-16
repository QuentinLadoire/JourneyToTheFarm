using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;

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
		}

		public bool CanAddItem(Item item, int amount)
		{
			return inventory.CanAddItem(item, amount);
		}

		public void AddItemAt(int displayIndex, int amount)
		{
			inventory.AddItemAt(displayIndex, amount);
		}
		public void RemoveItemAt(int displayIndex, int amount)
		{
			inventory.RemoveItemAt(displayIndex, amount);
		}

		public void AddItem(Item item, int amount)
		{
			inventory.AddItem(item, amount);
		}
		public void RemoveItem(Item item, int amount)
		{
			inventory.RemoveItem(item, amount);
		}

		public void MoveItem(int from, int to)
		{
			inventory.MoveItem(from, to);
		}
		public void MoveItem(int from, Inventory otherInventory, int to)
		{
			inventory.MoveItem(from, otherInventory, to);
		}

		public void SwapItem(int from, int to)
		{
			inventory.SwapItem(from, to);
		}
		public void SwapItem(int from, Inventory otherInventory, int to)
		{
			inventory.SwapItem(from, otherInventory, to);
		}
	}
}
