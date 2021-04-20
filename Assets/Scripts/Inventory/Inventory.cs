using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class ItemContainer
	{
		public const int AmountMax = 999;

		public Item Item { get; set; }
		public int Amount { get; set; }
		public int SlotIndex { get; set; }

		public ItemContainer(int slotIndex = -1, Item item = null, int amount = 0)
		{
			SlotIndex = slotIndex;
			Item = item;
			Amount = amount;
		}
	}

	public class Inventory : MonoBehaviour
	{
		public Action<ItemContainer> onScroll = (ItemContainer itemContainer) => { /*Debug.Log("OnScrollUp");*/ };
		public Action<ItemContainer> onAddItem = (ItemContainer itemContainer) => { /*Debug.Log("OnNewItemShortcut index : " + index);*/ };

		readonly ItemContainer[] slotShortcut = new ItemContainer[10];
		int shortcutIndex = 0;

		List<ItemContainer> usedSlots = new List<ItemContainer>();

		void ScrollUp()
		{
			shortcutIndex--;
			if (shortcutIndex == -1)
				shortcutIndex = 9;

			onScroll.Invoke(slotShortcut[shortcutIndex]);
		}
		void ScrollDown()
		{
			shortcutIndex++;
			if (shortcutIndex == 10)
				shortcutIndex = 0;

			onScroll.Invoke(slotShortcut[shortcutIndex]);
		}
		void ScrollInput()
		{
			float delta = Input.GetAxis("ScrollShortcut");

			if (delta > 0.0f)
				ScrollUp();
			else if (delta < 0.0f)
				ScrollDown();
		}

		bool AddItemAsEmptySlot(Item item, int amount = 1)
		{
			amount = Mathf.Clamp(amount, 1, ItemContainer.AmountMax);

			foreach (var slot in slotShortcut)
				if (slot.Item == null)
				{
					slot.Item = item;
					slot.Amount = amount;

					usedSlots.Add(slot);

					onAddItem.Invoke(slot);

					return true;
				}

			return false;
		}
		int FillItem(Item item, int amount = 1)
		{
			foreach (var slot in usedSlots)
			{
				if (amount > 0 && slot.Item == item)
				{
					var rest = ItemContainer.AmountMax - slot.Amount;
					if (amount < rest)
					{
						slot.Amount += amount;
						amount = 0;
					}
					else
					{
						slot.Amount = ItemContainer.AmountMax;
						amount -= rest;
					}

					onAddItem.Invoke(slot);
				}
			}

			return amount;
		}

		public bool HasEmptySlot()
		{
			return usedSlots.Count < slotShortcut.Length;
		}
		public bool AddItem(Item item, int amount = 1)
		{
			if (!item.stackable)
			{
				return AddItemAsEmptySlot(item);
			}
			else
			{
				amount = FillItem(item, amount);

				if (amount > 0)
					return AddItemAsEmptySlot(item, amount);

				return true;
			}
		}

		private void Awake()
		{
			for (int i = 0; i < slotShortcut.Length; i++)
				slotShortcut[i] = new ItemContainer(i);
		}
		private void Start()
		{
			onScroll.Invoke(slotShortcut[shortcutIndex]);
		}
		private void Update()
		{
			if (Player.HasControl)
			{
				ScrollInput();
			}
		}
	}
}
