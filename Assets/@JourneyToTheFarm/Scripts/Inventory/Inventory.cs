using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Inventory : MonoBehaviour
    {
        public Item[] ItemArray => itemArray;

        public int sizeMax = 0;

        Item[] itemArray = null;

        public bool AddItem(Item item)
		{
            if (item.IsStackable)
			{
                for (int i = 0; i < sizeMax; i++)
                    if (itemArray[i] != null && itemArray[i].name == item.name)
                    {
                        itemArray[i].amount += item.amount;
                        return true;
                    }
			}

            for (int i = 0; i < sizeMax; i++)
                if (itemArray[i] == null)
                {
                    itemArray[i] = item;
                    return true;
                }

            return false;
		}
        public bool RemoveItem(Item item)
		{
            for (int i = 0; i < sizeMax; i++)
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

		private void Awake()
		{
            itemArray = new Item[sizeMax];
            for (int i = 0; i < sizeMax; i++)
                itemArray[i] = null;
		}
	}
}
