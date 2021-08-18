using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Collectible : CustomBehaviour
    {
		[Header("Collectible Parameters")]
		[SerializeField] string itemName = "NoName";
		[SerializeField] ItemType itemType = ItemType.None;

		public string ItemName => itemName;
		public ItemType ItemType => itemType;

		void Collect(Player player)
		{
			if (!player.ShortcutController.AddItem(new Item(ItemName, ItemType, 1)) &&
				!player.InventoryController.AddItem(new Item(ItemName, ItemType, 1)))
				return;

			Destroy();
		}

		private void OnCollisionEnter(Collision collision)
		{
			var player = collision.gameObject.GetComponent<Player>();
			if (player != null)
				Collect(player);
		}

		public void SetCollectible(string itemName)
		{
			this.itemName = itemName;
		}
	}
}
