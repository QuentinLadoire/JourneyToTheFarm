using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class GameManager : MonoBehaviour
	{
		static GameManager instance = null;

		public static DataBase DataBase { get => instance.dataBase; }

		[SerializeField] DataBase dataBase = null;

		void OnInventoryOpen()
		{
			Cursor.lockState = CursorLockMode.Confined;

			Player.DesactiveControl();
		}
		void OnInventoryClose()
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
			Player.OnInventoryOpen -= OnInventoryOpen;
			Player.OnInventoryClose -= OnInventoryClose;
		}
	}
}
