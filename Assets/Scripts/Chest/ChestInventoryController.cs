using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ChestInventoryController : InventoryController
    {
        public static Action<InventoryController> onOpenInventory = (InventoryController controller) => { /*Debug.Log("OnOpenInventory");*/ };
        public static Action<InventoryController> onCloseInventory = (InventoryController controller) => { /*Debug.Log("OnCloseInventory");*/ };

        public override void OpenInventory()
		{
            onOpenInventory.Invoke(this);
		}
        public override void CloseInventory()
		{
            onCloseInventory.Invoke(this);
		}
	}
}
