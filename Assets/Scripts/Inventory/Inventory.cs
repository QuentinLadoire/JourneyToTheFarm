using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class ItemContainer
	{
		public const int AmountMax = 999;

		public bool IsFull { get => Amount == AmountMax; }
		public bool IsEmpty { get => Amount == 0; }

		public int Amount { get; set; } = 0;
		public string ItemName { get; set; } = "NoName";
		public ItemType ItemType { get; set; } = ItemType.None;
	}

	public class Inventory : MonoBehaviour
	{
		public const int sizeMax = 40;

		public Action<int, string, int, ItemType> onAddItem = (int index, string name, int amount, ItemType itemType) => { /*Debug.Log("OnAddItem");*/ };
		public Action<int, string, int, ItemType> onRemoveItem = (int index, string name, int amount, ItemType itemType) => { /*Debug.Log("OnRemoveItem");*/ };

		readonly ItemContainer[] slots = new ItemContainer[sizeMax];

		public int AddItem(string name, int amount, ItemType itemType)
		{
			var index = GetItemIndex(name, true);
			if (index != -1)
			{
				var rest = slots[index].Amount + amount - ItemContainer.AmountMax;
				AddItemAt(index, amount, itemType);

				if (rest > 0)
					return AddItem(name, rest, itemType);
				else
					return 0;
			}
			else
			{
				var emptySlotIndex = GetEmptySlotIndex();
				if (emptySlotIndex != -1)
				{
					var rest = amount - ItemContainer.AmountMax;
					SetItemAt(emptySlotIndex, name, amount, itemType);

					if (rest > 0)
						return AddItem(name, rest, itemType);
					else
						return 0;
				}
			}

			return amount;
		}
		public void RemoveItem(string name, int amount)
		{
			var index = GetItemIndex(name);
			if (index != -1)
			{
				var rest = amount - slots[index].Amount;
				RemoveItemAt(index, amount);

				if (rest > 0)
					RemoveItem(name, amount);
			}
			else
			{
				Debug.Log("Can't remove Item - ItemName : " + name + " - Amount : " + amount + " - Doesn't contain Item");
			}
		}

		public bool HasItem(string name, int amount)
		{
			var index = GetItemIndex(name);
			if (index != -1 && slots[index].Amount >= amount)
				return true;

			return false;
		}
		public int HowManyItem(string name)
		{
			var index = GetItemIndex(name);
			if (index != -1)
				return slots[index].Amount;

			return 0;
		}

		public string GetItemName(int index)
		{
			return slots[index].ItemName;
		}
		public ItemType GetItemType(int index)
		{
			return slots[index].ItemType;
		}
		public int GetItemAmount(int index)
		{
			return slots[index].Amount;
		}

		int GetEmptySlotIndex()
		{
			for (int i = 0; i < sizeMax; i++)
				if (slots[i].IsEmpty)
					return i;

			return -1;
		}
		int GetItemIndex(string name, bool ignoreFullSlot = false)
		{
			for (int i = 0; i < sizeMax; i++)
			{
				if (!ignoreFullSlot && slots[i].ItemName == name ||
					ignoreFullSlot && slots[i].ItemName == name && !slots[i].IsFull)
					return i;
			}

			return -1;
		}

		void ClearItemAt(int index)
		{
			slots[index].ItemName = "NoName";
			slots[index].Amount = 0;
			slots[index].ItemType = ItemType.None;

			onRemoveItem.Invoke(index, slots[index].ItemName, slots[index].Amount, ItemType.None);
		}
		void SetItemAt(int index, string name, int amount, ItemType itemType)
		{
			slots[index].ItemName = name;
			slots[index].Amount = amount;
			slots[index].ItemType = itemType;

			if (slots[index].Amount > ItemContainer.AmountMax)
				slots[index].Amount = ItemContainer.AmountMax;

			onAddItem.Invoke(index, name, amount, itemType);
		}

		void AddItemAt(int index, int amount, ItemType itemType)
		{
			slots[index].Amount += amount;
			if (slots[index].Amount > ItemContainer.AmountMax)
				slots[index].Amount = ItemContainer.AmountMax;

			onAddItem.Invoke(index, slots[index].ItemName, slots[index].Amount, itemType);
		}
		void RemoveItemAt(int index, int amount)
		{
			slots[index].Amount -= amount;
			if (slots[index].Amount <= 0)
				ClearItemAt(index);
			else
				onRemoveItem.Invoke(index, slots[index].ItemName, slots[index].Amount, slots[index].ItemType);
		}

		void SwapItem(int index1, int index2)
		{
			var tmp = slots[index1];
			slots[index1] = slots[index2];
			slots[index2] = tmp;
		}

		private void Awake()
		{
			for (int i = 0; i < sizeMax; i++)
				slots[i] = new ItemContainer();
		}
	}
}
