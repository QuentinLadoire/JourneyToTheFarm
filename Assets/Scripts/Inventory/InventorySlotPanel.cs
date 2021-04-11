using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class InventorySlotPanel : MonoBehaviour
	{
		[SerializeField] InventorySlot[] inventorySlots = null;
		int selectedSlot = 0;

		void OnAddItemShortcut(int index, ItemContainer itemContainer)
		{
			if (itemContainer.Item != null)
				inventorySlots[index].SetSprite(itemContainer.Item.sprite);
		}
		void OnScroll(int index, ItemContainer itemContainer)
		{
			inventorySlots[selectedSlot].SetSelected(false);
			inventorySlots[index].SetSelected(true);

			selectedSlot = index;
		}

		private void Awake()
		{
			Player.OnAddItemShortcut += OnAddItemShortcut;
			Player.OnScroll += OnScroll;
		}
		private void Start()
		{
			inventorySlots[selectedSlot].SetSelected(true);
		}
		private void OnDestroy()
		{
			Player.OnAddItemShortcut -= OnAddItemShortcut;
			Player.OnScroll -= OnScroll;
		}
	}
}
