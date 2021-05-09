using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class PlayerInventoryController : InventoryController
	{
		public Action<InventoryController> onOpenInventory = (InventoryController controller) => { /*Debug.Log("OnOpenInventory");*/ };
		public Action<InventoryController> onCloseInventory = (InventoryController controller) => { /*Debug.Log("OnCloseInventory");*/ };
		public Action<int, string, ItemType, int> onScroll = (int index, string itemName, ItemType itemType, int amount) => { /*Debug.Log("OnScroll");*/ };

		int shortcutIndex = 0;
		bool isOpen = false;

		public override void OpenInventory()
		{
			onOpenInventory.Invoke(this);
		}
		public override void CloseInventory()
		{
			onCloseInventory.Invoke(this);
		}

		void ScrollUp()
		{
			shortcutIndex--;
			if (shortcutIndex == -1)
				shortcutIndex = 9;

			onScroll.Invoke(shortcutIndex, inventory.GetItemName(shortcutIndex), inventory.GetItemType(shortcutIndex), inventory.GetItemAmount(shortcutIndex));
		}
		void ScrollDown()
		{
			shortcutIndex++;
			if (shortcutIndex == 10)
				shortcutIndex = 0;

			onScroll.Invoke(shortcutIndex, inventory.GetItemName(shortcutIndex), inventory.GetItemType(shortcutIndex), inventory.GetItemAmount(shortcutIndex));
		}
		void ScrollInput()
		{
			float delta = Input.GetAxis("ScrollShortcut");

			if (delta > 0.0f)
				ScrollUp();
			else if (delta < 0.0f)
				ScrollDown();
		}
		void OpeningInput()
		{
			if (Input.GetButtonDown("Inventory"))
			{
				isOpen = !isOpen;
				if (isOpen)
					OpenInventory();
				else
					CloseInventory();
			}
		}

		private void Start()
		{
			onScroll.Invoke(0, inventory.GetItemName(0), inventory.GetItemType(0), inventory.GetItemAmount(0));
		}
		private void Update()
		{
			OpeningInput();

			if (Player.HasControl)
			{
				ScrollInput();
			}
		}
	}
}
