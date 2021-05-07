using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ChestInventoryController : InventoryController
    {
        public static Action<ChestInventoryController> onOpenInventory = (ChestInventoryController controller) => { /*Debug.Log("OnOpenInventory");*/ };
        public static Action<ChestInventoryController> onCloseInventory = (ChestInventoryController controller) => { /*Debug.Log("OnCloseInventory");*/ };

        public void OpenInventory()
		{
            onOpenInventory.Invoke(this);
		}
        public void CloseInventory()
		{
            onCloseInventory.Invoke(this);
		}
	}
}
