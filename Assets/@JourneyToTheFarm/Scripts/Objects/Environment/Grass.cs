using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Grass : CustomBehaviour
    {
        [Header("Grass Parameters")]
        [Range(0.0f, 1.0f)] [SerializeField] float dropRate = 1.0f;

        public void Harvest(Player player)
		{
            if (Random.value < dropRate)
            {
                var collectiblePrefab = new Item("WheatSeed", ItemType.Seed, 1).CollectiblePrefab;
                if (collectiblePrefab != null)
                {
                    var amount = Random.Range(1, 6);
                    for (int i = 0; i < amount; i++)
                    {
                        var collectible = Instantiate(collectiblePrefab);
                        collectible.transform.position = transform.position + Vector3.up * 0.5f;
                    }
                }
            }

            Destroy();
		}
    }
}
