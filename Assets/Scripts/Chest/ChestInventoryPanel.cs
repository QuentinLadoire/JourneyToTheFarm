using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class ChestInventoryPanel : InventoryPanel
    {
		protected override void OnInventoryOpen(InventoryController controller)
		{
			base.OnInventoryOpen(controller);

			inventoryController.onAddItem += OnAddItem;
			inventoryController.onRemoveItem += OnRemoveItem;

			SetupPanel();
		}
		protected override void OnInventoryClose(InventoryController controller)
		{
			inventoryController.onAddItem -= OnAddItem;
			inventoryController.onRemoveItem -= OnRemoveItem;
			ClearPanel();

			base.OnInventoryClose(controller);
		}

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
