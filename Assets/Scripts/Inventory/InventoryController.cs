using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class InventoryController : MonoBehaviour
    {
		public Action<int, ItemInfo> onAddItem = (int index, ItemInfo info) => { /*Debug.Log("OnAddItem");*/ };
		public Action<int, ItemInfo> onRemoveItem = (int index, ItemInfo info) => { /*Debug.Log("OnRemoveItem");*/ };

        protected Inventory inventory = null;

		public void AddItemAt(int index, ItemInfo info)
		{
			inventory.AddItemAt(index, info.name, info.type, info.amount);

			onAddItem.Invoke(index, info);
		}
		public void AddItem(ItemInfo info)
		{
			int rest = info.amount;

			//Fill all identical item name slot, if exist
			for (int i = 0; i < inventory.GetSize(); i++)
			{
				if (rest > 0 && !inventory.IsEmptyAt(i) && !inventory.IsFull(i) && inventory.GetItemName(i) == info.name)
				{
					var free = ItemContainer.AmountMax - inventory.GetItemAmount(i);
					if (rest <= free)
					{
						inventory.AddItemAt(i, rest);
						rest = 0;

						onAddItem.Invoke(i, new ItemInfo(inventory.GetItemName(i), inventory.GetItemType(i), inventory.GetItemAmount(i)));
					}
					else
					{
						inventory.AddItemAt(i, free);
						rest -= free;

						onAddItem.Invoke(i, new ItemInfo(inventory.GetItemName(i), inventory.GetItemType(i), inventory.GetItemAmount(i)));
					}
				}
			}

			//Fill empty slot if all identical slot has full or does not exist 
			if (rest > 0)
			{
				for (int i = 0; i < inventory.GetSize(); i++)
				{
					if (rest > 0 && inventory.IsEmptyAt(i))
					{
						var free = ItemContainer.AmountMax;
						if (rest <= free)
						{
							inventory.AddItemAt(i, info.name, info.type, rest);
							rest = 0;

							onAddItem.Invoke(i, new ItemInfo(inventory.GetItemName(i), inventory.GetItemType(i), inventory.GetItemAmount(i)));
						}
						else
						{
							inventory.AddItemAt(i, info.name, info.type, free);
							rest -= free;

							onAddItem.Invoke(i, new ItemInfo(inventory.GetItemName(i), inventory.GetItemType(i), inventory.GetItemAmount(i)));
						}
					}
				}
			}
		}

		public void RemoveItemAt(int index)
		{
			var itemInfo = new ItemInfo(inventory.GetItemName(index), inventory.GetItemType(index), inventory.GetItemAmount(index));
			inventory.RemoveItemAt(index);

			onRemoveItem.Invoke(index, itemInfo);
		}
		public void RemoveItem(ItemInfo info)
		{
			var amountContain = inventory.HowManyItem(info.name);
			if (amountContain >= info.amount)
			{
				var restToRemove = info.amount;
				for (int i = 0; i < inventory.GetSize(); i++)
				{
					if (restToRemove > 0 && !inventory.IsEmptyAt(i) && inventory.GetItemName(i) == info.name)
					{
						if (inventory.GetItemAmount(i) >= restToRemove)
						{
							inventory.RemoveItemAt(i, restToRemove);
							restToRemove = 0;

							onRemoveItem.Invoke(i, new ItemInfo(inventory.GetItemName(i), inventory.GetItemType(i), inventory.GetItemAmount(i)));
						}
						else
						{
							restToRemove -= inventory.GetItemAmount(i);
							inventory.RemoveItemAt(i);

							onRemoveItem.Invoke(i, new ItemInfo(inventory.GetItemName(i), inventory.GetItemType(i), inventory.GetItemAmount(i)));
						}
					}
				}
			}
		}

		public bool HasItemAmount(string name, int amount)
		{
			return inventory.HasItemAmount(name, amount);
		}
		public ItemInfo GetItemInfoAt(int index)
		{
			return new ItemInfo(inventory.GetItemName(index), inventory.GetItemType(index), inventory.GetItemAmount(index));
		}
		public int HowManyItem(string name)
		{
			return inventory.HowManyItem(name);
		}

		public int GetInventorySize()
		{
			return inventory.GetSize();
		}

		private void Awake()
		{
			inventory = GetComponent<Inventory>();
		}
	}
}
