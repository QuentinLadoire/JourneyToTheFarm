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

		void OnInventoryOpen(InventoryController inventoryController)
		{
			Cursor.lockState = CursorLockMode.Confined;

			Player.DesactiveControl();
		}
		void OnInventoryClose(InventoryController inventoryController)
		{
			Cursor.lockState = CursorLockMode.Locked;

			Player.ActiveControl();
		}

		void OnCraftingOpen()
		{
			Cursor.lockState = CursorLockMode.Confined;

			Player.DesactiveControl();
		}
		void OnCraftingClose()
		{
			Cursor.lockState = CursorLockMode.Locked;

			Player.ActiveControl();
		}

		void OnOpenChestInventory(Chest chest)
		{
			Cursor.lockState = CursorLockMode.Confined;

			Player.DesactiveControl();
		}
		void OnCloseChestInventory(Chest chest)
		{
			Cursor.lockState = CursorLockMode.Locked;

			Player.ActiveControl();
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

			Chest.OnOpenChestInventory += OnOpenChestInventory;
			Chest.OnCloseChestInventory += OnCloseChestInventory;

			Player.AddItem("Shovel", 1, ItemType.Tool);
			Player.AddItem("WheatSeedBag", 1, ItemType.SeedBag);
			Player.AddItem("Axe", 1, ItemType.Tool);
			Player.AddItem("Pickaxe", 1, ItemType.Tool);
			Player.AddItem("Chest", 1, ItemType.Container);
		}
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				Player.AddItem("Log", 999, ItemType.Resource);
			}
		}
		private void OnDestroy()
		{
			Player.OnInventoryOpen -= OnInventoryOpen;
			Player.OnInventoryClose -= OnInventoryClose;

			Player.OnCraftingOpen -= OnCraftingOpen;
			Player.OnCraftingClose -= OnCraftingClose;

			Chest.OnOpenChestInventory -= OnOpenChestInventory;
			Chest.OnCloseChestInventory -= OnCloseChestInventory;
		}
	}
}
