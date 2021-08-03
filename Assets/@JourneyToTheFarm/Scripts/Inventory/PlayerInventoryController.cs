using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class PlayerInventoryController : MonoBehaviour
    {
		public Player Player { get; private set; }
		public Inventory Inventory => inventory;

		readonly Inventory inventory = new Inventory(30);
		bool isOpen = false;

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

		public bool AddItem(Item item)
		{
			return inventory.AddItem(item);
		}
		public bool RemoveItem(Item item)
		{
			return inventory.RemoveItem(item);
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

		private void Awake()
		{
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
	}
}
