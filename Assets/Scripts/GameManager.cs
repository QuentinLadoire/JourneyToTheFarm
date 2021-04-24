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

		void OnGameMenuOpen()
		{
			Cursor.lockState = CursorLockMode.Confined;

			Player.DesactiveControl();
		}
		void OnGameMenuClose()
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

			Player.OnInventoryOpen += OnGameMenuOpen;
			Player.OnInventoryClose += OnGameMenuClose;

			Player.OnCraftingOpen += OnGameMenuOpen;
			Player.OnCraftingClose += OnGameMenuClose;


			Player.AddItem(ItemType.Tool, "Shovel");
			Player.AddItem(ItemType.SeedBag, "WheatSeedBag");
			Player.AddItem(ItemType.Tool, "Axe");
			Player.AddItem(ItemType.Tool, "Pickaxe");
		}
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				Player.AddItem(ItemType.Resource, "Log", 999);
			}
		}
		private void OnDestroy()
		{
			Player.OnInventoryOpen -= OnGameMenuOpen;
			Player.OnInventoryClose -= OnGameMenuClose;

			Player.OnCraftingOpen -= OnGameMenuOpen;
			Player.OnCraftingClose -= OnGameMenuClose;
		}
	}
}
