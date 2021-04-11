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

		public static Action<int, ItemContainer> OnAddItemShortcut { get => instance.inventory.onAddItemShortcut; set => instance.inventory.onAddItemShortcut = value; }
		public static Action<int, ItemContainer> OnScroll { get => instance.inventory.onScroll; set => instance.inventory.onScroll = value; }

		public static void AddItem(ItemType itemType, string itemName)
		{
			instance.inventory.AddItem(GameManager.DataBase.GetItem(itemType, itemName));
		}

		CharacterController characterController = null;
        UsableController itemController = null;
        AnimationController animationController = null;
		Inventory inventory = null;

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			itemController = GetComponent<UsableController>();
			animationController = GetComponent<AnimationController>();
			inventory = GetComponent<Inventory>();

			instance = this;
		}
		private void Start()
		{
			AddItem(ItemType.Tool, "Shovel");
			AddItem(ItemType.SeedBag, "WheatSeedBag");
		}
	}
}
