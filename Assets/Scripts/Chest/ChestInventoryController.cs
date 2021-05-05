using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ChestInventoryController : MonoBehaviour
    {
        public static Action<ChestInventoryController> onOpenInventory = (ChestInventoryController controller) => { /*Debug.Log("OnOpenInventory");*/ };
        public static Action<ChestInventoryController> onCloseInventory = (ChestInventoryController controller) => { /*Debug.Log("OnCloseInventory");*/ };

		public Inventory Inventory => inventory;

        protected Inventory inventory = null;

        public void OpenInventory()
		{
            onOpenInventory.Invoke(this);
		}
        public void CloseInventory()
		{
            onCloseInventory.Invoke(this);
		}

		public int AddItem(string name, ItemType itemType, int amount)
		{
			return inventory.AddItem(name, amount, itemType);
		}

		private void Awake()
		{
			inventory = GetComponent<Inventory>();
		}
		private void Start()
		{
			AddItem("Log", ItemType.Resource, 999);
			AddItem("Stone", ItemType.Resource, 999);
			AddItem("WheatSeedBag", ItemType.SeedBag, 999);
			AddItem("Chest", ItemType.Container, 999);
		}
	}
}
