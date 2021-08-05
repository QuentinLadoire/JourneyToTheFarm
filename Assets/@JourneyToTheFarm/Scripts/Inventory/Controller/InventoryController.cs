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
		public Action onInventoryChange = () => { Debug.Log("OnInventoryChange"); };

		protected virtual void Awake()
		{
			inventory = new Inventory(InventorySize);
		}

		public bool AddItem(Item item)
		{
			bool value = inventory.AddItem(item);
			onInventoryChange.Invoke();

			return value;
		}
		public bool RemoveItem(Item item)
		{
			bool value = inventory.RemoveItem(item);
			onInventoryChange.Invoke();

			return value;
		}
	}
}
