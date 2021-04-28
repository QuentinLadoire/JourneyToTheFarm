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
		void OnScroll(int index, string name, ItemType itemType)
		{
			inventorySlots[selectedSlot].SetSelected(false);
			inventorySlots[index].SetSelected(true);

			selectedSlot = index;
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
