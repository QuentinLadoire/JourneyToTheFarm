using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Player : MonoBehaviour
    {
		static Player instance = null;

		public static Vector3 Position { get => instance.transform.position; }
		public static Vector3 Forward { get => instance.transform.forward; }
		public static Vector3 RoundPosition { get => instance.characterController.RoundPosition; }

		public static Action<Vector3> OnHasMoved { get => instance.characterController.onHasMoved; set => instance.characterController.onHasMoved = value; }

		public static Action<ItemContainer> OnAddItem { get => instance.inventory.onAddItem; set => instance.inventory.onAddItem = value; }
		public static Action<ItemContainer> OnScroll { get => instance.inventory.onScroll; set => instance.inventory.onScroll = value; }

		public static bool AddItem(ItemType itemType, string itemName, int amount = 1)
		{
			return instance.inventory.AddItem(GameManager.DataBase.GetItem(itemType, itemName), amount);
		}

		CharacterController characterController = null;
		Inventory inventory = null;

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			inventory = GetComponent<Inventory>();

			instance = this;
		}
		private void Start()
		{
			AddItem(ItemType.Tool, "Shovel");
			AddItem(ItemType.SeedBag, "WheatSeedBag");
			AddItem(ItemType.Tool, "Axe");
		}
	}
}
