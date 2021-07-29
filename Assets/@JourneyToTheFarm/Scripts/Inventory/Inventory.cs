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
		[SerializeField] int size = 40;
		ItemContainer[] slots = null;

		public void AddItemAt(int index, string name, ItemType itemType, int amount)
		{
			slots[index].ItemName = name;
			slots[index].ItemType = itemType;
			slots[index].Amount = amount;
		}
		public void AddItemAt(int index, int amount)
		{
			slots[index].Amount += amount;
		}
		public void RemoveItemAt(int index)
		{
			slots[index].ItemName = "None";
			slots[index].ItemType = ItemType.None;
			slots[index].Amount = 0;
		}
		public void RemoveItemAt(int index, int amount)
		{
			if (slots[index].Amount - amount <= 0)
				RemoveItemAt(index);
			else
				slots[index].Amount -= amount;
		}

		public int GetSize()
		{
			return slots.Length;
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
		public int GetItemIndex(string name)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				var slot = slots[i];
				if (!slot.IsEmpty && slot.ItemName == name)
					return i;
			}

			return -1;
		}
		public int GetEmptySlotIndex()
		{
			for (int i = 0; i < slots.Length; i++)
			{
				var slot = slots[i];
				if (slot.IsEmpty)
					return i;
			}

			return -1;
		}

		public bool IsEmptyAt(int index)
		{
			return slots[index].IsEmpty;
		}
		public bool IsFull(int index)
		{
			return slots[index].IsFull;
		}

		public bool HasEmptySlot()
		{
			foreach (var slot in slots)
				if (slot.IsEmpty)
					return true;

			return false;
		}
		public int HowManyItem(string name)
		{
			int itemAmount = 0;
			foreach (var slot in slots)
				if (!slot.IsEmpty && slot.ItemName == name)
					itemAmount += slot.Amount;

			return itemAmount;
		}
		public bool HasItemAmount(string name, int amount)
		{
			return (HowManyItem(name) >= amount);
		}
		public int HowManyItemCanAdd(string name)
		{
			int howManyEmptySlot = 0;
			int howManyItemAmount = 0;
			foreach (var slot in slots)
				if (slot.IsEmpty)
					howManyEmptySlot++;
				else if (!slot.IsFull && slot.ItemName == name)
					howManyItemAmount += ItemContainer.AmountMax - slot.Amount;

			return howManyItemAmount + howManyEmptySlot * ItemContainer.AmountMax;
		}

		private void Awake()
		{
			slots = new ItemContainer[size];
			for (int i = 0; i < size; i++)
				slots[i] = new ItemContainer();
		}
	}
}
