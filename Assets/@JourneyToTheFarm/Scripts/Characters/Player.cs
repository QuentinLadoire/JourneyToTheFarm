using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Player : CustomNetworkBehaviour
    {
        public MovementController CharacterController { get; private set; }
        public AnimationController AnimationController { get; private set; }
        public PlayerInventoryController InventoryController { get; private set; }
        public ShortcutInventoryController ShortcutController { get; private set; }
        public UsableObjectController UsableObjectController { get; private set; }
        public InteractableController InteractableController { get; private set; }
        public EquipableController EquipableController { get; private set; }

        protected override void Awake()
		{
            base.Awake();

            GameManager.player = this;

            CharacterController = GetComponent<MovementController>();
            AnimationController = GetComponent<AnimationController>();
            InventoryController = GetComponent<PlayerInventoryController>();
            ShortcutController = GetComponent<ShortcutInventoryController>();
            UsableObjectController = GetComponent<UsableObjectController>();
            InteractableController = GetComponent<InteractableController>();
            EquipableController = GetComponent<EquipableController>();
		}
		public override void NetworkStart()
		{
			base.NetworkStart();

            if (!(this.IsClient && this.IsLocalPlayer))
            {
                this.enabled = false;
                return;
            }
        }
		protected override void Start()
		{
            base.Start();

            ShortcutController.AddItem(new Item("Shovel", ItemType.Tool, 1));
            ShortcutController.AddItem(new Item("Axe", ItemType.Tool, 1));
            ShortcutController.AddItem(new Item("Pickaxe", ItemType.Tool, 1));
            ShortcutController.AddItem(new Item("Scythe", ItemType.Tool, 1));
            ShortcutController.AddItem(new Item("WheatPacket", ItemType.SeedPacket, 4));
            ShortcutController.AddItem(new Item("Chest", ItemType.Container, 1));
            ShortcutController.AddItem(new Item("Wheat", ItemType.Resource, 50));
        }

		public bool AddItem(Item item)
		{
            return ShortcutController.AddItem(item) || InventoryController.AddItem(item);
		}
	}
}
