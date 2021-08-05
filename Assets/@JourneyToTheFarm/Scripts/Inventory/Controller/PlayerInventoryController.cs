using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class PlayerInventoryController : InventoryController, IOpenable, IClosable
	{
		bool isOpen = false;

		protected override int InventorySize => 30;

		public Player Player { get; private set; } = null;

		protected override void Awake()
		{
			base.Awake();

			Player = GetComponent<Player>();
		}
		private void Start()
		{
			inventory.AddItem(new Item("Shovel", ItemType.Tool, 1));
			inventory.AddItem(new Item("Axe", ItemType.Tool, 1));
			inventory.AddItem(new Item("Pickaxe", ItemType.Tool, 1));
			inventory.AddItem(new Item("Scythe", ItemType.Tool, 1));
			inventory.AddItem(new Item("WheatSeed", ItemType.Seed, 20));
			inventory.AddItem(new Item("Log", ItemType.Resource, 50));
			inventory.AddItem(new Item("Stone", ItemType.Resource, 50));
		}
		private void Update()
		{
			ProcessInput();
		}

		void ProcessInput()
		{
			if (Input.GetButtonDown("Inventory"))
			{
				if (!isOpen)
					OpenInventory();
				else
					CloseInventory();
			}
		}

		public void OpenInventory()
		{
			isOpen = true;
			CanvasManager.GamePanel.OpenPlayerInventory(this);
			GameManager.ActiveCursor();
			Player.CharacterController.DesactiveControl();
		}
		public void CloseInventory()
		{
			isOpen = false;
			CanvasManager.GamePanel.ClosePlayerInventory();
			GameManager.DesactiveCursor();
			Player.CharacterController.ActiveControl();
		}
	}
}
