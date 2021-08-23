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

			for (int i = 0; i < stoneQuantity; i++)
			{
				World.DropItem(new Item(stoneName, ItemType.Resource, 1), transform.position + Vector3.up * 0.5f);
			}
		}
	}
}
