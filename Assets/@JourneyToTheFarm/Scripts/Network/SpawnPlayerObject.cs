using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

namespace JTTF
{
    public class SpawnPlayerObject : CustomNetworkBehaviour
    {
		[ServerRpc(RequireOwnership = false)]
		private void SpawnPlayerObjectServerRpc(ulong clientId)
		{
			var playerPrefab = NetworkManager.Singleton.NetworkConfig.NetworkPrefabs.Find(item => item.PlayerPrefab == true);
			if (playerPrefab != null)
			{
				var netObject = Instantiate(playerPrefab.Prefab).GetComponent<NetworkObject>();
				netObject.SpawnAsPlayerObject(clientId);
			}
		}

		public override void NetworkStart()
		{
			base.NetworkStart();

			if (!IsClient) return;

			SpawnPlayerObjectServerRpc(NetworkManager.Singleton.LocalClientId);

			SetActive(false);
		}
	}
}
