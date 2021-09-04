using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class World : MonoBehaviour
    {
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
    }
}
