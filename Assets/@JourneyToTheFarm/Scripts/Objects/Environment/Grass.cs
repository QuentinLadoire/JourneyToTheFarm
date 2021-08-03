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
                player.InventoryController.AddItem(new Item("WheatSeed", ItemType.Seed, Random.Range(1, 6)));

            Destroy();
		}
    }
}
