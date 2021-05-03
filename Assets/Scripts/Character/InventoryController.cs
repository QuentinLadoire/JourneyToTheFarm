using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class InventoryController : MonoBehaviour
	{
		public Action onOpen = () => { /*Debug.Log("OnOpen");*/ };
		public Action onClose = () => { /*Debug.Log("OnClose");*/ };

		public Action<int, string, ItemType, int> onScroll = (int index, string itemName, ItemType itemType, int amount) => { /*Debug.Log("OnScroll");*/ };

		int shortcutIndex = 0;
		bool isOpen = false;

		Inventory inventory = null;

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
					onOpen.Invoke();
				else
					onClose.Invoke();
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
