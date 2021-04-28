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


			Player.AddItem("Shovel", 1, ItemType.Tool);
			Player.AddItem("WheatSeedBag", 1, ItemType.SeedBag);
			Player.AddItem("Axe", 1, ItemType.Tool);
			Player.AddItem("Pickaxe", 1, ItemType.Tool);
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
			Player.OnInventoryOpen -= OnGameMenuOpen;
			Player.OnInventoryClose -= OnGameMenuClose;

			Player.OnCraftingOpen -= OnGameMenuOpen;
			Player.OnCraftingClose -= OnGameMenuClose;
		}
	}
}
