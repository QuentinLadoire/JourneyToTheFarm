using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF.Inventory
{
    public class Inventory
    {
        private readonly Item[] itemArray = null;

        public Item[] ItemArray => itemArray;
        public int SizeMax => itemArray.Length;

        private bool IndexIsGood(int index)
		{
            return index >= 0 && index < SizeMax;
		}
        private void ClearItemAt(int index)
		{
            itemArray[index] = Item.None;
		}

        public bool AddItem(Item item)
		{
            if (item.IsStackable)
			{
                for (int i = 0; i < SizeMax; i++)
                    if (itemArray[i] != Item.None && itemArray[i].name == item.name)
                    {
                        itemArray[i].amount += item.amount;
                        return true;
                    }
			}

            for (int i = 0; i < SizeMax; i++)
                if (itemArray[i] == Item.None)
                {
                    itemArray[i] = item;
                    return true;
                }

            return false;
		}
        public bool RemoveItem(Item item)
		{
            for (int i = 0; i < SizeMax; i++)
			{
                if (itemArray[i] != null && itemArray[i].name == item.name)
                {
                    itemArray[i].amount -= item.amount;
                    if (itemArray[i].amount <= 0)
                        itemArray[i] = Item.None;

                    return true;
                }
			}

            return false;
		}

        public bool AddItemAt(int index, Item item)
		{
            if (IndexIsGood(index))
            {
                ItemArray[index] = item;
                return true;
            }

            return false;
		}
        public bool RemoveItemAt(int index)
		{
            if (IndexIsGood(index))
			{
                ClearItemAt(index);
                return true;
			}

            return false;
		}

        public int StackItemAt(int index, int amount)
		{
            if (IndexIsGood(index) && itemArray[index].IsStackable)
			{
                itemArray[index].amount += amount;
                if (itemArray[index].amount > itemArray[index].StackCount)
                {
                    var rest = itemArray[index].amount - itemArray[index].StackCount;
                    itemArray[index].amount = itemArray[index].StackCount;

                    return rest;
                }

                return 0;
			}

            return -1;
		}
        public int UnstackItemAt(int index, int amount)
		{
            if (IndexIsGood(index))
            {
                itemArray[index].amount -= amount;
                if (itemArray[index].amount <= 0)
				{
                    var rest = -itemArray[index].amount;
                    ClearItemAt(index);

                    return rest;
				}

                return 0;
            }

            return -1;
        }

		public Inventory(int size)
		{
            itemArray = new Item[size];
            itemArray.Fill(Item.None);
		}
	}
}
