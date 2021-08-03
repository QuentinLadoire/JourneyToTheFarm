using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Inventory
    {
        public Item[] ItemArray => itemArray;
        public int SizeMax => itemArray.Length;

        readonly Item[] itemArray = null;

        public bool AddItem(Item item)
		{
            if (item.IsStackable)
			{
                for (int i = 0; i < SizeMax; i++)
                    if (itemArray[i] != null && itemArray[i].name == item.name)
                    {
                        itemArray[i].amount += item.amount;
                        return true;
                    }
			}

            for (int i = 0; i < SizeMax; i++)
                if (itemArray[i] == null)
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
                        itemArray[i] = null;

                    return true;
                }
			}

            return false;
		}

		public Inventory(int size)
		{
            itemArray = new Item[size];
            itemArray.Fill(null);
		}
	}
}
