using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;
using JTTF.Behaviour;
using JTTF.Character;
using JTTF.Inventory;
using JTTF.Management;
using MLAPI;

#pragma warning disable IDE0044

namespace JTTF.Gameplay
{
    public class Collectible : CustomNetworkBehaviour
    {
		[Header("Collectible Parameters")]
		[SerializeField] private TriggerBehaviour trigger = null;
		[SerializeField] private string itemName = "NoName";
		[SerializeField] private ItemType itemType = ItemType.None;

		private new Rigidbody rigidbody = null;

		public string ItemName => itemName;
		public ItemType ItemType => itemType;
		public Rigidbody Rigidbody => rigidbody;

		private void Collect(Player player)
		{
			if (player.CanAddItem(new Item(ItemName, ItemType), 1))
			{
				player.AddItem(new Item(ItemName, ItemType), 1);
				Destroy();
			}
		}
		private void OnTriggerEnterCallback(Collider other)
		{
			var player = other.gameObject.GetComponentInParent<Player>();
			if (player != null)
				Collect(player);
		}

		protected override void Awake()
		{
			base.Awake();

			rigidbody = GetComponent<Rigidbody>();
		}
		public override void NetworkStart()
		{
			base.NetworkStart();

			if (!IsServer)
			{
				enabled = false;
				return;
			}
		}
		protected override void Start()
		{
			base.Start();

			trigger.onTriggerEnter += OnTriggerEnterCallback;
		}
	}
}
