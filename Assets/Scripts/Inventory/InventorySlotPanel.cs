using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class InventorySlotPanel : MonoBehaviour
	{
		[SerializeField] InventorySlot[] inventorySlots = null;
		int selectedSlot = 0;

		void OnAddItem(ItemContainer itemContainer)
		{
			if (itemContainer.Item != null)
			{
				inventorySlots[itemContainer.SlotIndex].SetSprite(itemContainer.Item.sprite);
				inventorySlots[itemContainer.SlotIndex].SetAmount(itemContainer.Amount);
			}
		}
		void OnScroll(ItemContainer itemContainer)
		{
			inventorySlots[selectedSlot].SetSelected(false);
			inventorySlots[itemContainer.SlotIndex].SetSelected(true);

			selectedSlot = itemContainer.SlotIndex;
		}

		private void Awake()
		{
			Player.OnAddItem += OnAddItem;
			Player.OnScroll += OnScroll;
		}
		private void Start()
		{
			inventorySlots[selectedSlot].SetSelected(true);
		}
		private void OnDestroy()
		{
			Player.OnAddItem -= OnAddItem;
			Player.OnScroll -= OnScroll;
		}
	}
}
