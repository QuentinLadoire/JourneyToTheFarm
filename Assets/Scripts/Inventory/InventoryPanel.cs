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
		PlayerInventoryController inventoryController = null;

		void OnClick()
		{
			if (inventoryController != null)
				inventoryController.CloseInventory();
		}

        void OnAddItem(int index, ItemInfo info)
		{
			if (index < 10) return;

			var item = GameManager.ItemDataBase.GetItem(info.type, info.name);
			inventorySlots[index - 10].SetSprite(item.sprite);
			inventorySlots[index - 10].SetAmount(info.amount);
		}
		void OnRemoveItem(int index, ItemInfo info)
		{
			if (index < 10) return;

			inventorySlots[index - 10].SetAmount(info.amount);
			if (info.amount == 0)
				inventorySlots[index - 10].SetSprite(null);
		}

		void OnInventoryOpen(PlayerInventoryController controller)
		{
			gameObject.SetActive(true);
			inventoryController = controller;
		}
		void OnInventoryClose(PlayerInventoryController controller)
		{
			gameObject.SetActive(false);
			inventoryController = null;
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
