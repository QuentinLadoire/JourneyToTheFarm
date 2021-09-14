using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.UI;
using JTTF.Character;

namespace JTTF.Inventory
{
	public class ShortcutInventoryController : InventoryController
	{
		private int currentIndex = 0;

		protected override int InventorySize => 10;

		public Player OwnerPlayer { get; private set; } = null;

		public Action<int, Item> onSelectedSlotChange = (int index, Item item) => { /*Debug.Log("OnScroll");*/ };

		private void ScrollUp()
		{
			currentIndex--;
			if (currentIndex == -1)
				currentIndex = Inventory.SizeMax - 1;

			onSelectedSlotChange.Invoke(currentIndex, Inventory.ItemArray[currentIndex]);
		}
		private void ScrollDown()
		{
			currentIndex++;
			if (currentIndex == Inventory.SizeMax)
				currentIndex = 0;

			onSelectedSlotChange.Invoke(currentIndex, Inventory.ItemArray[currentIndex]);
		}
		private void ProcessInput()
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
		public override void NetworkStart()
		{
			base.NetworkStart();

			if (!(this.IsClient && this.IsLocalPlayer))
			{
				this.enabled = false;
				return;
			}
		}
		protected override void Start()
		{
			base.Start();
			
			CanvasManager.GamePanel.InitShortcutInventory(this);
		}
		protected override void FirstFrameUpdate()
		{
			base.FirstFrameUpdate();
			
			onSelectedSlotChange.Invoke(currentIndex, Inventory.ItemArray[currentIndex]); //Call for Instantiate the item at game start, if item exist
		}
		protected override void Update()
		{
			base.Update();

			ProcessInput();
		}

		public void ConsumeSelectedItem()
		{
			UnstackItemAt(currentIndex, 1);
			if (Inventory.ItemArray[currentIndex] == Item.None)
				onSelectedSlotChange.Invoke(currentIndex, Inventory.ItemArray[currentIndex]);
		}
	}
}
