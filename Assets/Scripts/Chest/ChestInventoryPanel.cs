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

		void OnAddItem(int index, ItemInfo info)
		{
			var item = GameManager.ItemDataBase.GetItem(info.type, info.name);
			inventorySlots[index].SetSprite(item.sprite);
			inventorySlots[index].SetAmount(info.amount);
		}
		void OnRemoveItem(int index, ItemInfo info)
		{
			inventorySlots[index].SetAmount(info.amount);
			if (info.amount == 0)
				inventorySlots[index].SetSprite(null);
		}

		void OnChestInventoryOpen(ChestInventoryController controller)
		{
			SetActive(true);

			inventoryController = controller;
			inventoryController.onAddItem += OnAddItem;
			inventoryController.onRemoveItem += OnRemoveItem;

			SetupPanel();
		}
		void OnChestInventoryClose(ChestInventoryController controller)
		{
			SetActive(false);

			ClearPanel();

			inventoryController.onAddItem -= OnAddItem;
			inventoryController.onRemoveItem -= OnRemoveItem;
			inventoryController = null;
		}

		void SetupPanel()
		{
			for (int i = 0; i < inventoryController.GetInventorySize(); i++)
			{
				var itemInfo = inventoryController.GetItemInfoAt(i);
				var item = GameManager.ItemDataBase.GetItem(itemInfo.type, itemInfo.name);
				inventorySlots[i].SetSprite(item.sprite);
				inventorySlots[i].SetAmount(itemInfo.amount);
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
