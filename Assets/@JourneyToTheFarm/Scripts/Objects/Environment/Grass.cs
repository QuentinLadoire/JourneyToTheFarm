using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;
using JTTF.Behaviour;
using JTTF.Character;
using JTTF.Inventory;
using JTTF.Management;

#pragma warning disable IDE0044

namespace JTTF.Gameplay
{
    public class Grass : CustomBehaviour
    {
        [Header("Grass Parameters")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float dropRate = 1.0f;

        public void Harvest()
		{
            if (Random.value < dropRate)
            {
                var amount = Random.Range(1, 6);
                for (int i = 0; i < amount; i++)
                {
                    World.DropItem(new Item("WheatPacket", ItemType.SeedPacket), transform.position + Vector3.up * 0.5f);
                }
            }

            Destroy();
		}
    }
}
