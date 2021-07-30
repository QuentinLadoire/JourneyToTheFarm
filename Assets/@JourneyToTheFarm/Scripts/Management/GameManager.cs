using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class GameManager : MonoBehaviour
	{
		static GameManager instance = null;

		public static ItemDataBase ItemDataBase { get => instance.itemDataBase; }
		public static Camera playerCamera = null;

		[SerializeField] ItemDataBase itemDataBase = null;

		int openedPanelCount = 0;

		void OnInventoryOpen(InventoryController controller)
		{
			openedPanelCount++;

			Cursor.lockState = CursorLockMode.Confined;
			Player.DesactiveControl();
		}
		void OnInventoryClose(InventoryController controller)
		{
			openedPanelCount--;

			if (openedPanelCount == 0)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Player.ActiveControl();
			}
		}

		void OnCraftingOpen()
		{
			openedPanelCount++;

			Cursor.lockState = CursorLockMode.Confined;
			Player.DesactiveControl();
		}
		void OnCraftingClose()
		{
			openedPanelCount--;

			if (openedPanelCount == 0)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Player.ActiveControl();
			}
		}

		void OnOpenChestInventory(InventoryController controller)
		{
			openedPanelCount++;

			Cursor.lockState = CursorLockMode.Confined;
			Player.DesactiveControl();
		}
		void OnCloseChestInventory(InventoryController controller)
		{
			openedPanelCount--;

			if (openedPanelCount == 0)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Player.ActiveControl();
			}
		}

		private void Awake()
		{
			instance = this;
		}
		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;

			Player.OnInventoryOpen += OnInventoryOpen;
			Player.OnInventoryClose += OnInventoryClose;

			Player.OnCraftingOpen += OnCraftingOpen;
			Player.OnCraftingClose += OnCraftingClose;

			Chest.OnOpenInventory += OnOpenChestInventory;
			Chest.OnCloseInventory += OnCloseChestInventory;

			//Player.AddItem(new ItemInfo("Shovel", ItemType.Tool, 1));
			//Player.AddItem(new ItemInfo("WheatSeedBag", ItemType.SeedBag, 1));
			//Player.AddItem(new ItemInfo("Axe", ItemType.Tool, 1));
			//Player.AddItem(new ItemInfo("Pickaxe", ItemType.Tool, 1));
			//Player.AddItem(new ItemInfo("Chest", ItemType.Container, 1));
		}
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				Player.AddItem(new ItemInfo("Log", ItemType.Resource, 999));
			}
			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				Player.RemoveItem(new ItemInfo("Log", ItemType.Resource, 999));
			}
		}
		private void OnDestroy()
		{
			Player.OnInventoryOpen -= OnInventoryOpen;
			Player.OnInventoryClose -= OnInventoryClose;

			Player.OnCraftingOpen -= OnCraftingOpen;
			Player.OnCraftingClose -= OnCraftingClose;

			Chest.OnOpenInventory -= OnOpenChestInventory;
			Chest.OnCloseInventory -= OnCloseChestInventory;
		}
	}
}
