using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Rock : MonoBehaviour
    {
		public string stoneName = "Stone";
		public int stoneQuantity = 3;
		public GameObject modelObject = null;

		bool isHarvested = false;

		public bool IsHarvestable()
		{
			return !isHarvested;
		}
		public void Harvest()
		{
			isHarvested = true;
			Destroy(gameObject);

			Player.AddItem(new ItemInfo(stoneName, ItemType.Resource, stoneQuantity));
		}
	}
}
