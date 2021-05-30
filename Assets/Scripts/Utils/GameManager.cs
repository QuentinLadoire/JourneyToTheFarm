using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class GameManager : MonoBehaviour
	{
		static GameManager instance = null;

		public static ItemDataBase ItemDataBase { get => instance.itemDataBase; }

		[SerializeField] ItemDataBase itemDataBase = null;

		int openedPanelNB = 0;

		public static int GetConstructiblRaycastMask()
		{
			return LayerMask.GetMask("Ground", "Road", "Rock", "Tree", "Hill");
		}
		public static int GetConstructibleOverlapMask()
		{
			return LayerMask.GetMask("Road", "Rock", "Tree", "Pebble", "Grass", "Hill", "FarmPlot");
		}
		public static int GetPlantableRaycastMask()
		{
			return LayerMask.GetMask("FarmPlot");
		}

		void OnInventoryOpen(InventoryController controller)
		{
			openedPanelNB++;

			Cursor.lockState = CursorLockMode.Confined;
			Player.DesactiveControl();
		}
		void OnInventoryClose(InventoryController controller)
		{
			openedPanelNB--;

			if (openedPanelNB == 0)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Player.ActiveControl();
			}
		}

		void OnCraftingOpen()
		{
			openedPanelNB++;

			Cursor.lockState = CursorLockMode.Confined;
			Player.DesactiveControl();
		}
		void OnCraftingClose()
		{
			openedPanelNB--;

			if (openedPanelNB == 0)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Player.ActiveControl();
			}
		}

		void OnOpenChestInventory(InventoryController controller)
		{
			openedPanelNB++;

			Cursor.lockState = CursorLockMode.Confined;
			Player.DesactiveControl();
		}
		void OnCloseChestInventory(InventoryController controller)
		{
			openedPanelNB--;

			if (openedPanelNB == 0)
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
