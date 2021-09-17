using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable IDE0044
#pragma warning disable IDE0090
#pragma warning disable IDE0051

namespace JTTF.Management
{
    using JTTF.Inventory;

    public class InventoryManager : MonoBehaviour
    {
        private static InventoryManager instance = null;

        private List<Inventory> inventoryList = new List<Inventory>();

		private void Awake()
		{
			if (instance != null)
			{
				Destroy(gameObject);
				return;
			}

			instance = this;
		}

		public static void AddNewInventory(Inventory inventory)
		{
            instance.inventoryList.Add(inventory);
		}
        public static void RemoveInventory(Inventory inventory)
		{
            instance.inventoryList.Remove(inventory);
		}

		public static Inventory GetInventory(ulong networkObjectID, ulong networkBehaviourId)
		{
			foreach (var inventory in instance.inventoryList)
				if (inventory.NetworkObjectId == networkObjectID && inventory.NetworkBehaviourId == networkBehaviourId)
					return inventory;

			return null;
		}
    }
}
