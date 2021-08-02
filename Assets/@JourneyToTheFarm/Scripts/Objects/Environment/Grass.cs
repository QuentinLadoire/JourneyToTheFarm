using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Grass : CustomBehaviour
    {
        [Header("Grass Parameters")]
        [Range(0.0f, 1.0f)] public float dropRate = 1.0f;

        public void Harvest()
		{
            if (Random.value < dropRate)
                //Player.AddItem(new ItemInfo("WheatSeedBag", ItemType.SeedBag, Random.Range(1, 6)));

            Destroy();
		}
    }
}
