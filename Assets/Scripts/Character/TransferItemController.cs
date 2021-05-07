using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class TransferItemController : MonoBehaviour
    {
        PlayerInventoryController playerInventoryController = null;
        ChestInventoryController chestInventoryController = null;

		void OnOpenChestInventory(ChestInventoryController controller)
		{
			chestInventoryController = controller;
		}
		void OnCloseChestInventory(ChestInventoryController controller)
		{
			chestInventoryController = null;
		}

		private void Awake()
		{
			playerInventoryController = GetComponent<PlayerInventoryController>();

			Chest.OnOpenInventory += OnOpenChestInventory;
			Chest.OnCloseInventory += OnCloseChestInventory;
		}
		private void OnDestroy()
		{
			Chest.OnOpenInventory -= OnOpenChestInventory;
			Chest.OnCloseInventory -= OnCloseChestInventory;
		}
	}
}
