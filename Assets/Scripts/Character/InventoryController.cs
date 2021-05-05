using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class InventoryController : MonoBehaviour
	{
		public Action<InventoryController> onOpen = (InventoryController inventoryController) => { /*Debug.Log("OnOpen");*/ };
		public Action<InventoryController> onClose = (InventoryController inventoryController) => { /*Debug.Log("OnClose");*/ };

		public Action<int, string, ItemType, int> onScroll = (int index, string itemName, ItemType itemType, int amount) => { /*Debug.Log("OnScroll");*/ };

		int shortcutIndex = 0;
		bool isOpen = false;

		Inventory inventory = null;

		public void OpenInventory()
		{
			onOpen.Invoke(this);
		}
		public void CloseInventory()
		{
			onClose.Invoke(this);
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

		private void Awake()
		{
			inventory = GetComponent<Inventory>();
		}
		private void Start()
		{
			onScroll.Invoke(shortcutIndex, inventory.GetItemName(shortcutIndex), inventory.GetItemType(shortcutIndex), inventory.GetItemAmount(shortcutIndex));
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
