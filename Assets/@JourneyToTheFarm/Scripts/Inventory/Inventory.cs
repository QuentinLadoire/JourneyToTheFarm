using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;
using JTTF.Management;
using MLAPI.Serialization;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;

#pragma warning disable IDE0044
#pragma warning disable IDE0090

namespace JTTF.Inventory
{
    public class ItemContainer : INetworkSerializable
	{
        public int displayIndex = -1;
        public Item item = Item.None;
        public int amount = 0;

        public bool IsFull => !(amount < item.StackCount);
        public int FreeCount => Mathf.Clamp(item.StackCount - amount, 0, item.StackCount);

		public void NetworkSerialize(NetworkSerializer serializer)
		{
            serializer.Serialize(ref displayIndex);
            item.NetworkSerialize(serializer);
            serializer.Serialize(ref amount);
		}
	}

    public class Inventory : CustomNetworkBehaviour
    {
        private int sizeMax = 0;
        private bool[] displayIndexArray = null;
        private List<ItemContainer> itemContainerList = new List<ItemContainer>();
        private NetworkList<ItemContainer> itemContainerNetworkList = new NetworkList<ItemContainer>(new NetworkVariableSettings
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.ServerOnly
        });

        private IList<ItemContainer> ItemContainerList
        { 
            get
            {
                if (GameManager.IsMulti)
                    return itemContainerNetworkList;
                else
                    return itemContainerList;
            }
        }

        public int SizeMax => sizeMax;
        public int ItemCount => ItemContainerList.Count;
        
        public Action onInventoryChange = () => { /*Debug.Log("OnInventoryChange");*/ };

        private void OnItemContainerListChanged(NetworkListEvent<ItemContainer> changeEvent)
		{
            onInventoryChange.Invoke();
		}

        private bool IndexIsGood(int index)
		{
            return index >= 0 && index < ItemCount;
		}

        private void ClearItemContainerAt(int index)
		{
            SetDisplayIndexAt(ItemContainerList[index].displayIndex, true);
            ItemContainerList.RemoveAt(index);
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
            ItemContainerList.Add(itemContainer);
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

		protected override void Awake()
		{
            base.Awake();

            itemContainerNetworkList.OnListChanged += OnItemContainerListChanged;
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
                foreach (var container in ItemContainerList)
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
                ItemContainerList[index].amount += amount;
                if (ItemContainerList[index].amount > ItemContainerList[index].item.StackCount)
				{
                    ItemContainerList[index].amount = ItemContainerList[index].item.StackCount;
				}

                ItemContainerList[index] = new ItemContainer
                {
                    displayIndex = ItemContainerList[index].displayIndex,
                    item = ItemContainerList[index].item,
                    amount = ItemContainerList[index].amount
                };

                onInventoryChange.Invoke();
			}
		}
        public void RemoveItemAt(int displayIndex, int amount)
		{
            var index = GetIndex(displayIndex);
            if (index != -1)
			{
                ItemContainerList[index].amount -= amount;
                if (ItemContainerList[index].amount <= 0)
				{
                    ClearItemContainerAt(index);
				}
                else
				{
                    ItemContainerList[index] = new ItemContainer
                    {
                        displayIndex = ItemContainerList[index].displayIndex,
                        item = ItemContainerList[index].item,
                        amount = ItemContainerList[index].amount
                    };
                }

                onInventoryChange.Invoke();
			}
		}

        public void AddItem(Item item, int amount)
		{
            for (int i = 0; i < ItemCount; i++)
			{
                var itemContainer = ItemContainerList[i];
                if (itemContainer.item == item && !itemContainer.IsFull)
				{
                    itemContainer.amount += amount;
                    itemContainer.amount = Mathf.Clamp(itemContainer.amount, 0, itemContainer.item.StackCount);
                    amount = itemContainer.amount - itemContainer.item.StackCount;

                    ItemContainerList[i] = new ItemContainer
                    {
                        displayIndex = itemContainer.displayIndex,
                        item = itemContainer.item,
                        amount = itemContainer.amount
                    };
                                        
                    if (amount <= 0)
                        break;
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
                var itemContainer = ItemContainerList[i];
                if (itemContainer.item == item)
				{
                    itemContainer.amount -= amount;
                    if (itemContainer.amount <= 0)
					{
                        amount = -itemContainer.amount;
                        ClearItemContainerAt(i);
                        i--;
					}
                    else
					{
                        ItemContainerList[i] = new ItemContainer
                        {
                            displayIndex = itemContainer.displayIndex,
                            item = itemContainer.item,
                            amount = itemContainer.amount
                        };

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
                ItemContainerList[index].displayIndex = to;

                SetDisplayIndexAt(from, true);
                SetDisplayIndexAt(to, false);

                ItemContainerList[index] = new ItemContainer
                {
                    displayIndex = ItemContainerList[index].displayIndex,
                    item = ItemContainerList[index].item,
                    amount = ItemContainerList[index].amount
                };

                onInventoryChange.Invoke();
            }
		}
        public void MoveItem(int from, Inventory otherInventory, int to)
		{
            var index = GetIndex(from);
            if (index != -1)
            {
                otherInventory.AddItemContainer(to, ItemContainerList[index].item, ItemContainerList[index].amount);
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
                ItemContainerList[fromIndex].displayIndex = to;
                ItemContainerList[fromIndex] = new ItemContainer
                {
                    displayIndex = ItemContainerList[fromIndex].displayIndex,
                    item = ItemContainerList[fromIndex].item,
                    amount = ItemContainerList[fromIndex].amount
                };

                ItemContainerList[toIndex].displayIndex = from;
                ItemContainerList[toIndex] = new ItemContainer
                {
                    displayIndex = ItemContainerList[toIndex].displayIndex,
                    item = ItemContainerList[toIndex].item,
                    amount = ItemContainerList[toIndex].amount
                };

                onInventoryChange.Invoke();
            }
		}
        public void SwapItem(int from, Inventory otherInventory, int to)
		{
            var fromIndex = GetIndex(from);
            var toIndex = otherInventory.GetIndex(to);
            if (fromIndex != -1 && toIndex != -1)
            {
                var fromItem = ItemContainerList[fromIndex].item;
                var fromAmount = ItemContainerList[fromIndex].amount;

                var toItem = otherInventory.ItemContainerList[toIndex].item;
                var toAmount = otherInventory.ItemContainerList[toIndex].amount;

                ItemContainerList[fromIndex].item = toItem;
                ItemContainerList[fromIndex].amount = toAmount;
                ItemContainerList[fromIndex] = new ItemContainer
                {
                    displayIndex = ItemContainerList[fromIndex].displayIndex,
                    item = ItemContainerList[fromIndex].item,
                    amount = ItemContainerList[fromIndex].amount
                };

                otherInventory.ItemContainerList[toIndex].item = fromItem;
                otherInventory.ItemContainerList[toIndex].amount = fromAmount;
                otherInventory.ItemContainerList[toIndex] = new ItemContainer
                {
                    displayIndex = otherInventory.ItemContainerList[toIndex].displayIndex,
                    item = otherInventory.ItemContainerList[toIndex].item,
                    amount = otherInventory.ItemContainerList[toIndex].amount
                };

                onInventoryChange.Invoke();
                otherInventory.onInventoryChange.Invoke();
            }
        }

        public int GetIndex(int displayIndex)
		{
            for (int i = 0; i < ItemCount; i++)
			{
                var itemContainer = ItemContainerList[i];
                if (itemContainer.displayIndex == displayIndex)
                    return i;
			}

            return -1;
		}
        public Item GetItemAt(int index)
		{
            if (IndexIsGood(index))
			{
                return ItemContainerList[index].item;
			}

            return Item.None;
		}
        public int GetAmountAt(int index)
		{
            if (IndexIsGood(index))
			{
                return ItemContainerList[index].amount;
			}

            return 0;
		}

        public int GetDisplayIndex(int index)
		{
            if (IndexIsGood(index))
			{
                return ItemContainerList[index].displayIndex;
			}

            return -1;
		}
        public Item GetItemAtDisplayIndex(int displayIndex)
        {
            var index = GetIndex(displayIndex);
            if (index != -1)
            {
                return ItemContainerList[index].item;
            }

            return Item.None;
        }
        public int GetAmountAtDisplaIndex(int displayIndex)
        {
            var index = GetIndex(displayIndex);
            if (index != -1)
            {
                return ItemContainerList[index].amount;
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
