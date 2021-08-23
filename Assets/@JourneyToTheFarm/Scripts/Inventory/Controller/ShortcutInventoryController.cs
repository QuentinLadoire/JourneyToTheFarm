using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class ShortcutInventoryController : InventoryController
	{
		int currentIndex = 0;

		protected override int InventorySize => 10;

		public Player OwnerPlayer { get; private set; } = null;

		public Action<int, Item> onSelectedSlotChange = (int index, Item item) => { /*Debug.Log("OnScroll");*/ };

		void ScrollUp()
		{
			currentIndex--;
			if (currentIndex == -1)
				currentIndex = inventory.SizeMax - 1;

			onSelectedSlotChange.Invoke(currentIndex, inventory.ItemArray[currentIndex]);
		}
		void ScrollDown()
		{
			currentIndex++;
			if (currentIndex == inventory.SizeMax)
				currentIndex = 0;

			onSelectedSlotChange.Invoke(currentIndex, inventory.ItemArray[currentIndex]);
		}
		void ProcessInput()
		{
			var delta = Input.GetAxisRaw("ScrollShortcut");
			if (delta > 0.0f)
				ScrollUp();
			else if (delta < 0.0f)
				ScrollDown();
		}

		protected override void Awake()
		{
			base.Awake();

			OwnerPlayer = GetComponent<Player>();
		}
		private void Start()
		{
			CanvasManager.GamePanel.InitShortcutInventory(this);

			onSelectedSlotChange.Invoke(currentIndex, inventory.ItemArray[currentIndex]); //Call for Instantiate the item at game start, if item exist
		}
		private void Update()
		{
			ProcessInput();
		}

		public void ConsumeSelectedItem()
		{
			UnstackItemAt(currentIndex, 1);
			if (inventory.ItemArray[currentIndex] == Item.None)
				onSelectedSlotChange.Invoke(currentIndex, inventory.ItemArray[currentIndex]);
		}
	}
}
