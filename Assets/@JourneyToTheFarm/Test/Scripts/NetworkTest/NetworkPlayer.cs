using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF.Test
{
    public class NetworkPlayer : Player
    {
		public NetworkCharacterController NetworkCharacterController { get; private set; }
		public NetworkAnimationController NetworkAnimationController { get; private set; }
		public NetworkPlayerInventoryController NetworkPlayerInventoryController { get; private set; }
		public NetworkShortcutInventoryController NetworkShortcutInventoryController { get; private set; }
		public NetworkUsableObjectController NetworkUsableObjectController { get; private set; }
		public NetworkInteractableController NetworkInteractableController { get; private set; }
		public NetworkEquipableController NetworkEquipableController { get; private set; }

		protected override void Awake()
		{
			base.Awake();

			NetworkCharacterController = GetComponent<NetworkCharacterController>();
			NetworkAnimationController = GetComponent<NetworkAnimationController>();
			NetworkPlayerInventoryController = GetComponent<NetworkPlayerInventoryController>();
			NetworkShortcutInventoryController = GetComponent<NetworkShortcutInventoryController>();
			NetworkUsableObjectController = GetComponent<NetworkUsableObjectController>();
			NetworkInteractableController = GetComponent<NetworkInteractableController>();
			NetworkEquipableController = GetComponent<NetworkEquipableController>();
		}
	}
}

