using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ChestDebug : MonoBehaviour
    {
        InventoryController inventoryController = null;

		private void Awake()
		{
			inventoryController = GetComponent<InventoryController>();
		}
		private void Start()
		{
			inventoryController.AddItem(new ItemInfo("Shovel", ItemType.Tool, 1));
			inventoryController.AddItem(new ItemInfo("Axe", ItemType.Tool, 1));
			inventoryController.AddItem(new ItemInfo("Pickaxe", ItemType.Tool, 1));
			inventoryController.AddItem(new ItemInfo("WheatSeedBag", ItemType.SeedBag, 1));
		}
	}
}
