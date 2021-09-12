using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;
using JTTF.Behaviour;
using JTTF.Inventory;
using JTTF.Management;
using MLAPI.Messaging;

namespace JTTF.Gameplay
{
    public class Rock : CustomNetworkBehaviour
    {
		[SerializeField] private string stoneName = "Stone";
		[SerializeField] private int stoneQuantity = 3;

		private bool isHarvested = false;

		[ServerRpc(RequireOwnership = false)]
		private void HarvestServerRpc()
		{
			HarvestSolo();
		}
		private void HarvestSolo()
		{
			isHarvested = true;
			Destroy(gameObject);

			for (int i = 0; i < stoneQuantity; i++)
			{
				World.DropItem(new Item(stoneName, ItemType.Resource, 1), transform.position + Vector3.up * 0.5f);
			}
		}

		public bool IsHarvestable()
		{
			return !isHarvested;
		}
		public void Harvest()
		{
			if (GameManager.IsMulti)
				HarvestServerRpc();
			else
				HarvestSolo();
		}
	}
}
