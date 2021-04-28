using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField] InventorySlot[] inventorySlots = null;

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

		void OnInventoryOpen()
		{
			gameObject.SetActive(true);
		}
		void OnInventoryClose()
		{
			gameObject.SetActive(false);
		}

		public void Init()
		{
			Player.OnAddItem += OnAddItem;
			Player.OnRemoveItem += OnRemoveItem;

			Player.OnInventoryOpen += OnInventoryOpen;
			Player.OnInventoryClose += OnInventoryClose;
		}
		public void Destroy()
		{
			Player.OnAddItem -= OnAddItem;
			Player.OnRemoveItem -= OnRemoveItem;

			Player.OnInventoryOpen -= OnInventoryOpen;
			Player.OnInventoryClose -= OnInventoryClose;
		}
	}
}
