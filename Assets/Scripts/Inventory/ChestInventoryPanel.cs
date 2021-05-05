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
		Chest currentChest = null;

		void OnClick()
		{
			if (currentChest != null)
				currentChest.CloseChest();
		}

		void OnChestInventoryOpen(Chest chest)
		{
			gameObject.SetActive(true);

			currentChest = chest;

			SetupPanel();
		}
		void OnChestInventoryClose(Chest chest)
		{
			gameObject.SetActive(false);

			ClearPanel();

			currentChest = null;
		}

		void SetupPanel()
		{
			for (int i = 0; i < currentChest.Inventory.Slots.Length; i++)
			{
				var chestInventorySlot = currentChest.Inventory.Slots[i];
				var item = GameManager.ItemDataBase.GetItem(chestInventorySlot.ItemType, chestInventorySlot.ItemName);
				inventorySlots[i].SetSprite(item.sprite);
				inventorySlots[i].SetAmount(chestInventorySlot.Amount);
			}
		}
		void ClearPanel()
		{
			for (int i = 0; i < currentChest.Inventory.Slots.Length; i++)
			{
				var chestInventorySlot = currentChest.Inventory.Slots[i];
				var item = GameManager.ItemDataBase.GetItem(chestInventorySlot.ItemType, chestInventorySlot.ItemName);
				inventorySlots[i].SetSprite(null);
				inventorySlots[i].SetAmount(0);
			}
		}

		protected override void Awake()
		{
			base.Awake();

			closeButton.onClick.AddListener(OnClick);

			Chest.OnOpenChestInventory += OnChestInventoryOpen;
			Chest.OnCloseChestInventory += OnChestInventoryClose;
		}
        private void OnDestroy()
		{
			Chest.OnOpenChestInventory -= OnChestInventoryOpen;
			Chest.OnCloseChestInventory -= OnChestInventoryClose;
		}
    }
}
