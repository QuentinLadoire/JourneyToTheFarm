using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;
using JTTF.Behaviour;
using JTTF.Inventory;
using JTTF.Management;

namespace JTTF.Character
{
    public class Player : CustomNetworkBehaviour
    {
        public MovementController MovementController { get; private set; }
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

            MovementController = GetComponent<MovementController>();
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

            ShortcutController.AddItem(new Item("Shovel", ItemType.Tool), 1);
            ShortcutController.AddItem(new Item("Axe", ItemType.Tool), 1);
            ShortcutController.AddItem(new Item("Pickaxe", ItemType.Tool), 1);
            ShortcutController.AddItem(new Item("Scythe", ItemType.Tool), 1);
        }

        public bool CanAddItem(Item item, int amount)
		{
            return ShortcutController.CanAddItem(item, amount) || InventoryController.CanAddItem(item, amount);
		}

        public int GetAmountOf(Item item)
		{
            return ShortcutController.GetAmountOf(item) + InventoryController.GetAmountOf(item);
		}

		public void AddItem(Item item, int amount)
		{
            if (ShortcutController.CanAddItem(item, amount))
                ShortcutController.AddItem(item, amount);
            else if (InventoryController.CanAddItem(item, amount))
                InventoryController.AddItem(item, amount);
		}
        public void RemoveItem(Item item, int amount)
		{
            var shorcutAmount = ShortcutController.GetAmountOf(item);
            ShortcutController.RemoveItem(item, shorcutAmount);
            amount -= shorcutAmount;

            InventoryController.RemoveItem(item, amount);
		}
	}
}
