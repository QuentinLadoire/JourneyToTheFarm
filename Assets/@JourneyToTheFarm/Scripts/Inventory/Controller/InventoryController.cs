using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class InventoryController : MonoBehaviour
    {
		protected virtual int InventorySize => 0;

        protected Inventory inventory = null;

		public Inventory Inventory => inventory;
		public Action onInventoryChange = () => { /*Debug.Log("OnInventoryChange");*/ };

		protected virtual void Awake()
		{
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
	}
}
