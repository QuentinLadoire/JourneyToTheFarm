using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class PlayerInventoryController : MonoBehaviour
    {
		public Inventory Inventory => inventory;

        Inventory inventory = null;

		bool isOpen = false;

		public void OpenInventory()
		{
			isOpen = true;
			CanvasManager.GamePanel.OpenPlayerInventory(this);
		}
		public void CloseInventory()
		{
			isOpen = false;
			CanvasManager.GamePanel.ClosePlayerInventory();
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
			inventory = GetComponent<Inventory>();
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
