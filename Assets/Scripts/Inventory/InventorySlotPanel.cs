using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class InventorySlotPanel : MonoBehaviour
	{
		[SerializeField] InventorySlot[] inventorySlots = null;
		int selectedSlot = 0;

		void OnAddItem(int index, string name, int amount, ItemType itemType)
		{
			if (index >= 10) return;

			var item = GameManager.ItemDataBase.GetItem(itemType, name);
			inventorySlots[index].SetSprite(item.sprite);
			inventorySlots[index].SetAmount(amount);
		}
		void OnRemoveItem(int index, string name, int amount, ItemType itemType)
		{
			if (index >= 10) return;

			inventorySlots[index].SetAmount(amount);
			if (amount == 0)
				inventorySlots[index].SetSprite(null);
		}
		void OnScroll(int index, string name, ItemType itemType)
		{
			inventorySlots[selectedSlot].SetSelected(false);
			inventorySlots[index].SetSelected(true);

			selectedSlot = index;
		}

		private void Awake()
		{
			Player.OnAddItem += OnAddItem;
			Player.OnRemoveItem += OnRemoveItem;
			Player.OnScroll += OnScroll;
		}
		private void Start()
		{
			inventorySlots[selectedSlot].SetSelected(true);
		}
		private void OnDestroy()
		{
			Player.OnAddItem -= OnAddItem;
			Player.OnRemoveItem -= OnRemoveItem;
			Player.OnScroll -= OnScroll;
		}
	}
}
