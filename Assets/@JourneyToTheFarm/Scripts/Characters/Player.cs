using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Player : MonoBehaviour
    {
        public CharacterController CharacterController { get; private set; }
        public AnimationController AnimationController { get; private set; }
        public PlayerInventoryController InventoryController { get; private set; }
        public ShortcutInventoryController ShortcutController { get; private set; }
        public UsableObjectController UsableObjectController { get; private set; }
        public InteractableController InteractableController { get; private set; }
        public EquipableController EquipableController { get; private set; }

        private void Awake()
		{
            GameManager.player = this;

            CharacterController = GetComponent<CharacterController>();
            AnimationController = GetComponent<AnimationController>();
            InventoryController = GetComponent<PlayerInventoryController>();
            ShortcutController = GetComponent<ShortcutInventoryController>();
            UsableObjectController = GetComponent<UsableObjectController>();
            InteractableController = GetComponent<InteractableController>();
            EquipableController = GetComponent<EquipableController>();
		}
		private void Start()
		{
            ShortcutController.AddItem(new Item("Shovel", ItemType.Tool, 1));
            ShortcutController.AddItem(new Item("Axe", ItemType.Tool, 1));
            ShortcutController.AddItem(new Item("Pickaxe", ItemType.Tool, 1));
            ShortcutController.AddItem(new Item("Scythe", ItemType.Tool, 1));
            ShortcutController.AddItem(new Item("WheatPacket", ItemType.SeedPacket, 4));
            ShortcutController.AddItem(new Item("Chest", ItemType.Container, 1));
            ShortcutController.AddItem(new Item("Wheat", ItemType.Resource, 50));
        }
	}
}
