using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class InventoryController : CustomNetworkBehaviour
    {
		protected virtual int InventorySize => 0;

        protected Inventory inventory = null;

		public Inventory Inventory => inventory;
		public Action onInventoryChange = () => { /*Debug.Log("OnInventoryChange");*/ };

		protected override void Awake()
		{
			base.Awake();

			inventory = new Inventory(InventorySize);
		}

		public bool AddItem(Item item)
		{
			bool value = inventory.AddItem(item);
			if (value)
				onInventoryChange.Invoke();

			return value;
		}
		public bool RemoveItem(Item item)
		{
			bool value = inventory.RemoveItem(item);
			if (value)
				onInventoryChange.Invoke();

			return value;
		}

		public bool AddItemAt(int index, Item item)
		{
			var value = inventory.AddItemAt(index, item);
			if (value)
				onInventoryChange.Invoke();

			return value;
		}
		public bool RemoveItemAt(int index)
		{
			var value = inventory.RemoveItemAt(index);
			if (value)
				onInventoryChange.Invoke();

			return value;
		}

		public int StackItemAt(int index, int amount)
		{
			var value = inventory.StackItemAt(index, amount);
			if (value != -1)
				onInventoryChange.Invoke();

			return value;
		}
		public int UnstackItemAt(int index, int amount)
		{
			var value = inventory.UnstackItemAt(index, amount);
			if (value != -1)
				onInventoryChange.Invoke();

			return value;
		}

		public void SwitchItem(int index, InventoryController other, int otherIndex)
		{
			var toMove = inventory.ItemArray[index];
			AddItemAt(index, other.Inventory.ItemArray[otherIndex]);
			other.AddItemAt(otherIndex, toMove);
		}
	}
}
