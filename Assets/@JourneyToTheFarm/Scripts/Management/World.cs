using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Messaging;

namespace JTTF
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

        public static Collectible DropItem(Item item, Vector3 spawnPosition, Vector3 direction)
        {
            if (item != Item.None && item.CollectiblePrefab != null)
            {
                var collectible = Instantiate(item.CollectiblePrefab).GetComponent<Collectible>();
                collectible.transform.position = spawnPosition;
                collectible.Rigidbody.AddForce(direction * dropForce, ForceMode.Impulse);

                return collectible;
            }

            return null;
        }
        public static Collectible DropItem(Item item, Vector3 spawnPosition)
        {
            return DropItem(item, spawnPosition, GetRandomDirection());
        }

        public static void Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null) return;

            var hash = prefab.GetComponent<NetworkObject>().PrefabHash;
            instance.InstantiateServerRpc(hash, position, rotation);
        }

        [ServerRpc(RequireOwnership = false)]
        private void InstantiateServerRpc(ulong prefabHash, Vector3 position, Quaternion rotation)
        {
            var index = NetworkSpawnManager.GetNetworkPrefabIndexOfHash(prefabHash);
            var prefab = NetworkManager.NetworkConfig.NetworkPrefabs[index].Prefab;

            var obj = Instantiate(prefab).GetComponent<NetworkObject>();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.Spawn();
        }

        protected override void Awake()
        {
            base.Awake();

            instance = this;
        }
    }
}
