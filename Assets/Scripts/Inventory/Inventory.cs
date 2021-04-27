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
		public const int sizeMax = 40;

		public Action<ItemContainer> onScroll = (ItemContainer itemContainer) => { /*Debug.Log("OnScrollUp");*/ };
		public Action<ItemContainer> onAddItem = (ItemContainer itemContainer) => { /*Debug.Log("OnNewItemShortcut index : " + index);*/ };

		public Action onOpen = () => { /*Debug.Log("OnOpen");*/ };
		public Action onClose = () => { /*Debug.Log("OnClose");*/ };

		bool isOpen = false;

		readonly ItemContainer[] slotShortcut = new ItemContainer[sizeMax];
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
		void OpeningInput()
		{
			if (Input.GetButtonDown("Inventory"))
			{
				isOpen = !isOpen;
				if (isOpen)
					onOpen.Invoke();
				else
					onClose.Invoke();
			}
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
		public int HasItem(string name)
		{
			foreach (var usedSlot in usedSlots)
				if (usedSlot.Item.name == name)
					return usedSlot.Amount;

			return 0;
		}
		public void RemoveItem(string name, int amount = 1)
		{
			foreach (var usedSlot in usedSlots)
			{
				if (usedSlot.Item.name == name)
					if (usedSlot.Amount > amount)
						usedSlot.Amount -= amount;
					else
					{
						amount -= usedSlot.Amount;
						usedSlot.Amount = 0;
						if (amount == 0)
						{
							usedSlots.RemoveAll(item => item.Amount == 0);
							return;
						}
					}
			}

			usedSlots.RemoveAll(item => item.Amount == 0);
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
			OpeningInput();

			if (Player.HasControl)
			{
				ScrollInput();
			}
		}
	}
}
