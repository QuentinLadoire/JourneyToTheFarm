using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Collectible : CustomBehaviour
    {
		[Header("Collectible Parameters")]
		[SerializeField] TriggerBehaviour trigger = null;
		[SerializeField] string itemName = "NoName";
		[SerializeField] ItemType itemType = ItemType.None;

		new Rigidbody rigidbody = null;

		public string ItemName => itemName;
		public ItemType ItemType => itemType;
		public Rigidbody Rigidbody => rigidbody;

		void Collect(Player player)
		{
			if (!player.ShortcutController.AddItem(new Item(ItemName, ItemType, 1)) &&
				!player.InventoryController.AddItem(new Item(ItemName, ItemType, 1)))
				return;
			
			Destroy();
		}
		void OnTriggerEnterCallback(Collider other)
		{
			var player = other.gameObject.GetComponentInParent<Player>();
			if (player != null)
				Collect(player);
		}

		protected override void Awake()
		{
			base.Awake();

			rigidbody = GetComponent<Rigidbody>();

			trigger.onTriggerEnter += OnTriggerEnterCallback;
		}

		public void SetCollectible(string itemName)
		{
			this.itemName = itemName;
		}
	}
}
