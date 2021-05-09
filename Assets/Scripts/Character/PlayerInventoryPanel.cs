using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class PlayerInventoryPanel : InventoryPanel
    {
		protected override void Awake()
		{
			base.Awake();

			Player.OnAddItem += OnAddItem;
			Player.OnRemoveItem += OnRemoveItem;

			Player.OnInventoryOpen += OnInventoryOpen;
			Player.OnInventoryClose += OnInventoryClose;
		}
		private void OnDestroy()
		{
			Player.OnAddItem -= OnAddItem;
			Player.OnRemoveItem -= OnRemoveItem;

			Player.OnInventoryOpen -= OnInventoryOpen;
			Player.OnInventoryClose -= OnInventoryClose;
		}
	}
}
