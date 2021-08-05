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
	}
}
