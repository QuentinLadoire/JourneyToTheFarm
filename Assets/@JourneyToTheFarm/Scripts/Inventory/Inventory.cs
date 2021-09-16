using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;

#pragma warning disable IDE0044
#pragma warning disable IDE0090

namespace JTTF.Inventory
{
    public class ItemContainer
	{
        public int displayIndex = -1;
        public Item item = Item.None;
        public int amount = 0;

        public bool IsFull => !(amount < item.StackCount);
        public int FreeCount => Mathf.Clamp(item.StackCount - amount, 0, item.StackCount);
	}

    public class Inventory : CustomNetworkBehaviour
    {
        private int sizeMax = 0;
        private bool[] displayIndexArray = null;
        private List<ItemContainer> itemContainerList = new List<ItemContainer>();

        public int SizeMax => sizeMax;
        public int ItemCount => itemContainerList.Count;

        public Action onInventoryChange = () => { /*Debug.Log("OnInventoryChange");*/ };

        private bool IndexIsGood(int index)
		{
            return index >= 0 && index < ItemCount;
		}

        private void ClearItemContainerAt(int index)
		{
            SetDisplayIndexAt(itemContainerList[index].displayIndex, true);
            itemContainerList.RemoveAt(index);
		}
        private void AddItemContainer(int displayIndex, Item item, int amount)
		{
            SetDisplayIndexAt(displayIndex, false);
            var itemContainer = new ItemContainer
            {
                displayIndex = displayIndex,
                item = item,
                amount = amount
            };
            itemContainerList.Add(itemContainer);
		}

        private int GetFreeDisplayIndex()
		{
            for (int i = 0; i < SizeMax; i++)
                if (displayIndexArray[i])
                {
                    displayIndexArray[i] = false;
                    return i;
                }

            return -1;
		}
        private void SetDisplayIndexAt(int index, bool value)
		{
            displayIndexArray[index] = value;
		}

        public bool CanAddItem(Item item, int amount)
		{
            var containerCount = amount / item.StackCount;
            var freeContainerCount = sizeMax - ItemCount;
            if (containerCount < freeContainerCount)
			{
                return true;
			}
            else
			{
                foreach (var container in itemContainerList)
				{
                    if (container.item == item)
					{
                        amount -= container.FreeCount;
                        if (amount <= 0)
						{
                            return true;
						}
					}
				}

                return false;
			}
		}

        public void AddItemAt(int displayIndex, int amount)
		{
            var index = GetIndex(displayIndex);
            if (index != -1)
			{
                itemContainerList[index].amount += amount;
                if (itemContainerList[index].amount > itemContainerList[index].item.StackCount)
				{
                    itemContainerList[index].amount = itemContainerList[index].item.StackCount;
				}
                onInventoryChange.Invoke();
			}
		}
        public void RemoveItemAt(int displayIndex, int amount)
		{
            var index = GetIndex(displayIndex);
            if (index != -1)
			{
                itemContainerList[index].amount -= amount;
                if (itemContainerList[index].amount <= 0)
				{
                    ClearItemContainerAt(index);
				}
                onInventoryChange.Invoke();
			}
		}

        public void AddItem(Item item, int amount)
		{
            for (int i = 0; i < ItemCount; i++)
			{
                var itemContainer = itemContainerList[i];
                if (itemContainer.item == item && !itemContainer.IsFull)
				{
                    itemContainer.amount += amount;
                    if (itemContainer.amount > itemContainer.item.StackCount)
                    {
                        amount = itemContainer.amount - itemContainer.item.StackCount;
                        itemContainer.amount = itemContainer.item.StackCount;
                    }
                    else
                    {
                        break;
                    }
				}
			}

            if (amount > 0)
			{
                AddItemContainer(GetFreeDisplayIndex(), item, amount);
			}

            onInventoryChange.Invoke();
        }
        public void RemoveItem(Item item, int amount)
		{
            for (int i = 0; i < ItemCount; i++)
			{
                var itemContainer = itemContainerList[i];
                if (itemContainer.item == item)
				{
                    itemContainer.amount -= amount;
                    if (itemContainer.amount < 1)
					{
                        amount = -itemContainer.amount;
                        ClearItemContainerAt(i);
                        i--;
					}
                    else
					{
                        break;
					}
				}
			}

            onInventoryChange.Invoke();
        }

        public void MoveItem(int from, int to)
		{
            var index = GetIndex(from);
            if (index != -1)
            {
                itemContainerList[index].displayIndex = to;

                SetDisplayIndexAt(from, true);
                SetDisplayIndexAt(to, false);

                onInventoryChange.Invoke();
            }
		}
        public void MoveItem(int from, Inventory otherInventory, int to)
		{
            var index = GetIndex(from);
            if (index != -1)
            {
                otherInventory.AddItemContainer(to, itemContainerList[index].item, itemContainerList[index].amount);
                ClearItemContainerAt(index);

                onInventoryChange.Invoke();
                otherInventory.onInventoryChange.Invoke();
            }
		}

        public void SwapItem(int from, int to)
		{
            var fromIndex = GetIndex(from);
            var toIndex = GetIndex(to);

            if (fromIndex != -1 && toIndex != -1)
            {
                itemContainerList[fromIndex].displayIndex = to;
                itemContainerList[toIndex].displayIndex = from;

                onInventoryChange.Invoke();
            }
		}
        public void SwapItem(int from, Inventory otherInventory, int to)
		{
            var fromIndex = GetIndex(from);
            var toIndex = otherInventory.GetIndex(to);
            if (fromIndex != -1 && toIndex != -1)
            {
                var fromItem = itemContainerList[fromIndex].item;
                var fromAmount = itemContainerList[fromIndex].amount;

                var toItem = otherInventory.itemContainerList[toIndex].item;
                var toAmount = otherInventory.itemContainerList[toIndex].amount;

                itemContainerList[fromIndex].item = toItem;
                itemContainerList[fromIndex].amount = toAmount;

                otherInventory.itemContainerList[toIndex].item = fromItem;
                otherInventory.itemContainerList[toIndex].amount = fromAmount;

                onInventoryChange.Invoke();
                otherInventory.onInventoryChange.Invoke();
            }
        }

        public int GetIndex(int displayIndex)
		{
            for (int i = 0; i < ItemCount; i++)
			{
                var itemContainer = itemContainerList[i];
                if (itemContainer.displayIndex == displayIndex)
                    return i;
			}

            return -1;
		}
        public Item GetItemAt(int index)
		{
            if (IndexIsGood(index))
			{
                return itemContainerList[index].item;
			}

            return Item.None;
		}
        public int GetAmountAt(int index)
		{
            if (IndexIsGood(index))
			{
                return itemContainerList[index].amount;
			}

            return 0;
		}

        public int GetDisplayIndex(int index)
		{
            if (IndexIsGood(index))
			{
                return itemContainerList[index].displayIndex;
			}

            return -1;
		}
        public Item GetItemAtDisplayIndex(int displayIndex)
        {
            var index = GetIndex(displayIndex);
            if (index != -1)
            {
                return itemContainerList[index].item;
            }

            return Item.None;
        }
        public int GetAmountAtDisplaIndex(int displayIndex)
        {
            var index = GetIndex(displayIndex);
            if (index != -1)
            {
                return itemContainerList[index].amount;
            }

            return 0;
        }

        public void Init(int size)
		{
            sizeMax = size;
            displayIndexArray = new bool[size];
            displayIndexArray.Fill(true);
        }
	}
}
