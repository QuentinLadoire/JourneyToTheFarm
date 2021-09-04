using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Collectible : CustomBehaviour
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
			if (player.AddItem(new Item(ItemName, ItemType, 1)))
				Destroy();
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

			trigger.onTriggerEnter += OnTriggerEnterCallback;
		}

		public void SetCollectible(string itemName)
		{
			this.itemName = itemName;
		}
	}
}
