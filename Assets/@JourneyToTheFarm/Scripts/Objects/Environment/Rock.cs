using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Rock : MonoBehaviour
    {
		[SerializeField] private string stoneName = "Stone";
		[SerializeField] private int stoneQuantity = 3;

		private bool isHarvested = false;

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
