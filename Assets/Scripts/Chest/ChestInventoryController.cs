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

		private void Awake()
		{
			inventory = GetComponent<Inventory>();
		}
	}
}
