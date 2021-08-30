using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

namespace JTTF.Test
{
    public class NetworkPlayerInventoryController : PlayerInventoryController
    {
        private NetworkObject networkObject = null;
        private NetworkPlayer networkOwnerPlayer = null;

        public NetworkObject NetworkObject => networkObject;
        public NetworkPlayer NetworkOwnerPlayer => networkOwnerPlayer;

		protected override void Awake()
		{
			base.Awake();

			networkObject = GetComponent<NetworkObject>();
			networkOwnerPlayer = GetComponent<NetworkPlayer>();
		}
		protected virtual void Start()
		{
			if (!networkObject.IsLocalPlayer)
			{
				this.enabled = false;
				return;
			}
		}
	}
}
