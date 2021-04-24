using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class InventoryPanel : MonoBehaviour
    {
        [SerializeField] InventorySlot[] inventorySlots = null;

        void OnAddItem(ItemContainer itemContainer)
		{
			if (itemContainer.SlotIndex < Inventory.sizeMax && itemContainer.SlotIndex >= 10)
				if (itemContainer.Item != null)
				{
					inventorySlots[itemContainer.SlotIndex - 10].SetSprite(itemContainer.Item.sprite);
					inventorySlots[itemContainer.SlotIndex - 10].SetAmount(itemContainer.Amount);
				}
		}

		public void SetActive(bool value)
		{
			gameObject.SetActive(value);
		}
		public void Init()
		{
			Player.OnAddItem += OnAddItem;
		}
		public void Destroy()
		{
			Player.OnAddItem -= OnAddItem;
		}
	}
}
