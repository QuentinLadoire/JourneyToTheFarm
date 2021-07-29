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

			Player.OnInventoryOpen += OnInventoryOpen;
			Player.OnInventoryClose += OnInventoryClose;
		}
		private void OnDestroy()
		{
			Player.OnInventoryOpen -= OnInventoryOpen;
			Player.OnInventoryClose -= OnInventoryClose;
		}
	}
}
