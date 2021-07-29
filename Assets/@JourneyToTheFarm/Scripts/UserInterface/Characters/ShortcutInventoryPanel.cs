using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ShortcutInventoryPanel : InventoryPanel
    {
		[SerializeField] GameObject selectedImage = null;

		void OnScroll(int index, string itemName, ItemType itemType, int amount)
		{
			if (selectedImage != null)
				selectedImage.transform.position = inventorySlots[index].transform.position;
		}

		protected override void Awake()
		{
			base.Awake();

			inventoryController = Player.InventoryController;

			Player.OnAddItem += OnAddItem;
			Player.OnRemoveItem += OnRemoveItem;
			Player.OnScroll += OnScroll;
		}
		private void OnDestroy()
		{
			Player.OnAddItem -= OnAddItem;
			Player.OnRemoveItem -= OnRemoveItem;
			Player.OnScroll -= OnScroll;
		}
	}
}
