using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Gameplay;
using JTTF.Behaviour;
using JTTF.Inventory;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Messaging;

namespace JTTF.Management
{
    public class World : CustomNetworkBehaviour
    {
        private static World instance = null;

        public const float dropForce = 5.0f;

        private static Vector3 GetRandomDirection()
        {
            var direction = Random.insideUnitSphere;
            direction.y = 0.9f;
            direction.Normalize();

            return direction;
        }

        public static void DropItem(Item item, Vector3 spawnPosition, Vector3 direction)
        {
            if (item != Item.None && item.CollectiblePrefab != null)
            {
                if (GameManager.IsMulti)
                {
                    var prefabHash = item.CollectiblePrefab.GetComponent<NetworkObject>().PrefabHash;
                    instance.SpawnCollectibleServerRpc(prefabHash, spawnPosition, Quaternion.identity, direction);
                }
                else
                {
                    var collectible = Instantiate(item.CollectiblePrefab, spawnPosition, Quaternion.identity).GetComponent<Collectible>();
                    collectible.Rigidbody.AddForce(direction * dropForce, ForceMode.Impulse);
                }
            }
        }
        public static void DropItem(Item item, Vector3 spawnPosition)
        {
            DropItem(item, spawnPosition, GetRandomDirection());
        }

        public static void SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null) return;

            if (GameManager.IsMulti)
            {
                var hash = prefab.GetComponent<NetworkObject>().PrefabHash;
                instance.SpawnObjectServerRpc(hash, position, rotation);
            }
            else
			{
                MonoBehaviour.Instantiate(prefab, position, rotation);
			}
        }
        public static void SpawnPlayerObject(Vector3 position, Quaternion rotation)
		{
            if (GameManager.IsMulti)
			{
                instance.SpawnPlayerObjectServerRpc(NetworkManager.Singleton.LocalClientId, position, rotation);
            }
            else
			{
                MonoBehaviour.Instantiate(GameManager.PrefabDataBase.FarmerControllerPrefab, position, rotation);
			}
		}

        [ServerRpc(RequireOwnership = false)]
        private void SpawnObjectServerRpc(ulong prefabHash, Vector3 position, Quaternion rotation)
        {
            var index = NetworkSpawnManager.GetNetworkPrefabIndexOfHash(prefabHash);
            var prefab = NetworkManager.NetworkConfig.NetworkPrefabs[index].Prefab;

            var netObject = MonoBehaviour.Instantiate(prefab, position, rotation).GetComponent<NetworkObject>();
            netObject.Spawn();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnPlayerObjectServerRpc(ulong clientId, Vector3 position, Quaternion rotation)
        {
            var playerPrefab = NetworkManager.Singleton.NetworkConfig.NetworkPrefabs.Find(item => item.PlayerPrefab == true);
            if (playerPrefab != null)
            {
                var netObject = MonoBehaviour.Instantiate(playerPrefab.Prefab, position, rotation).GetComponent<NetworkObject>();
                netObject.SpawnAsPlayerObject(clientId);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnCollectibleServerRpc(ulong prefabHash, Vector3 position, Quaternion rotation, Vector3 direction)
		{
            var index = NetworkSpawnManager.GetNetworkPrefabIndexOfHash(prefabHash);
            var prefab = NetworkManager.NetworkConfig.NetworkPrefabs[index].Prefab;

            var netObject = MonoBehaviour.Instantiate(prefab, position, rotation).GetComponent<NetworkObject>();
            netObject.Spawn();

            var collectible = netObject.GetComponent<Collectible>();
            collectible.Rigidbody.AddForce(direction * dropForce, ForceMode.Impulse);
        }

        protected override void Awake()
        {
            base.Awake();

            instance = this;
        }
    }
}
