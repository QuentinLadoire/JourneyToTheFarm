using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ShortcutInventoryPanel : InventoryPanel
    {
		protected override void Awake()
		{
			base.Awake();

			inventoryController = Player.InventoryController;

			Player.OnAddItem += OnAddItem;
			Player.OnRemoveItem += OnRemoveItem;
		}
		private void OnDestroy()
		{
			Player.OnAddItem -= OnAddItem;
			Player.OnRemoveItem -= OnRemoveItem;
		}
	}
}
