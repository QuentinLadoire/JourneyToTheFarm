using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class InventorySlotPanel : MonoBehaviour
	{
		[SerializeField] InventorySlot[] inventorySlots = null;
		int selectedSlot = 0;

		void OnAddItemShortcut(int index, Item item)
		{
			inventorySlots[index].SetSprite(item.sprite);
		}
		void OnScroll(int index, Item item)
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
