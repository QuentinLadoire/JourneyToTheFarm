using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class InventoryPanel : SimpleObject
    {
		[SerializeField] Button closeButton = null;
		[SerializeField] InventorySlot[] inventorySlots = null;

		InventoryController invController = null;

		void OnClick()
		{
			if (invController != null)
				invController.CloseInventory();
		}

        void OnAddItem(int index, string name, int amount, ItemType itemType)
		{
			if (index < 10) return;

			var item = GameManager.ItemDataBase.GetItem(itemType, name);
			inventorySlots[index - 10].SetSprite(item.sprite);
			inventorySlots[index - 10].SetAmount(amount);
		}
		void OnRemoveItem(int index, string name, int amount, ItemType itemType)
		{
			if (index < 10) return;

			inventorySlots[index - 10].SetAmount(amount);
			if (amount == 0)
				inventorySlots[index - 10].SetSprite(null);
		}

		void OnInventoryOpen(InventoryController inventoryController)
		{
			gameObject.SetActive(true);
			invController = inventoryController;
		}
		void OnInventoryClose(InventoryController inventoryController)
		{
			gameObject.SetActive(false);
		}

		protected override void Awake()
		{
			base.Awake();

			closeButton.onClick.AddListener(OnClick);

			Player.OnAddItem += OnAddItem;
			Player.OnRemoveItem += OnRemoveItem;

			Player.OnInventoryOpen += OnInventoryOpen;
			Player.OnInventoryClose += OnInventoryClose;
		}
		private void OnDestroy()
		{
			Player.OnAddItem -= OnAddItem;
			Player.OnRemoveItem -= OnRemoveItem;

			Player.OnInventoryOpen -= OnInventoryOpen;
			Player.OnInventoryClose -= OnInventoryClose;
		}
	}
}
