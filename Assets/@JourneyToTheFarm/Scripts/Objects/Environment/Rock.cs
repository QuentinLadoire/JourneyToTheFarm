using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Rock : MonoBehaviour
    {
		[SerializeField] string stoneName = "Stone";
		[SerializeField] int stoneQuantity = 3;

		bool isHarvested = false;

		public bool IsHarvestable()
		{
			return !isHarvested;
		}
		public void Harvest(Player player)
		{
			isHarvested = true;
			Destroy(gameObject);

			player.InventoryController.AddItem(new Item(stoneName, ItemType.Resource, stoneQuantity));
		}
	}
}
