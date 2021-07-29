using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class ChestInventoryPanel : InventoryPanel
    {
		protected override void Awake()
		{
			base.Awake();

			Chest.OnOpenInventory += OnInventoryOpen;
			Chest.OnCloseInventory += OnInventoryClose;
		}
        private void OnDestroy()
		{
			Chest.OnOpenInventory -= OnInventoryOpen;
			Chest.OnCloseInventory -= OnInventoryClose;
		}
    }
}
