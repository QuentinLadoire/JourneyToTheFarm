using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;
using JTTF.Behaviour;
using JTTF.Inventory;
using JTTF.Management;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

#pragma warning disable IDE0044

namespace JTTF.Gameplay
{
    public class Rock : CustomNetworkBehaviour
    {
		[SerializeField] private string stoneName = "Stone";
		[SerializeField] private int stoneQuantity = 3;

		private NetworkVariableFloat scaleSync = new NetworkVariableFloat(new NetworkVariableSettings
		{
			ReadPermission = NetworkVariablePermission.Everyone,
			WritePermission = NetworkVariablePermission.ServerOnly
		});
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
				World.DropItem(new Item(stoneName, ItemType.Resource), transform.position + Vector3.up * 0.5f);
			}
		}

		protected override void Start()
		{
			base.Start();

			if (GameManager.IsMulti && NetworkManager.IsServer)
			{
				scaleSync.Value = transform.localScale.x;
			}
			if (GameManager.IsMulti && !NetworkManager.IsServer)
			{
				scaleSync.OnValueChanged += (previousValue, newValue) =>
				{
					transform.localScale = new Vector3(newValue, newValue, newValue);
				};
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
