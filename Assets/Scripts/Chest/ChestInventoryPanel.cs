using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class ChestInventoryPanel : SimpleObject
    {
		[SerializeField] Button closeButton = null;
        [SerializeField] InventorySlot[] inventorySlots = null;
		ChestInventoryController inventoryController = null;

		void OnClick()
		{
			if (inventoryController != null)
				inventoryController.CloseInventory();
		}

		void OnChestInventoryOpen(ChestInventoryController controller)
		{
			SetActive(true);

			inventoryController = controller;

			SetupPanel();
		}
		void OnChestInventoryClose(ChestInventoryController controller)
		{
			SetActive(false);

			ClearPanel();

			inventoryController = null;
		}

		void SetupPanel()
		{
			for (int i = 0; i < inventoryController.Inventory.Slots.Length; i++)
			{
				var chestInventorySlot = inventoryController.Inventory.Slots[i];
				var item = GameManager.ItemDataBase.GetItem(chestInventorySlot.ItemType, chestInventorySlot.ItemName);
				inventorySlots[i].SetSprite(item.sprite);
				inventorySlots[i].SetAmount(chestInventorySlot.Amount);
			}
		}
		void ClearPanel()
		{
			for (int i = 0; i < inventorySlots.Length; i++)
			{
				inventorySlots[i].SetSprite(null);
				inventorySlots[i].SetAmount(0);
			}
		}

		protected override void Awake()
		{
			base.Awake();

			closeButton.onClick.AddListener(OnClick);

			Chest.OnOpenInventory += OnChestInventoryOpen;
			Chest.OnCloseInventory += OnChestInventoryClose;
		}
        private void OnDestroy()
		{
			Chest.OnOpenInventory -= OnChestInventoryOpen;
			Chest.OnCloseInventory -= OnChestInventoryClose;
		}
    }
}
