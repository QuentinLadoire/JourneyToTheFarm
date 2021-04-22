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

		public static bool HasControl { get => instance.hasControl; }
		public static Action OnActiveControl { get => instance.onActiveControl; set => instance.onActiveControl = value; }
		public static Action OnDesactiveControl { get => instance.onDesactiveControl; set => instance.onDesactiveControl = value; }

		public static Action<Vector3> OnHasMoved { get => instance.characterController.onHasMoved; set => instance.characterController.onHasMoved = value; }

		public static Action<ItemContainer> OnAddItem { get => instance.inventory.onAddItem; set => instance.inventory.onAddItem = value; }
		public static Action<ItemContainer> OnScroll { get => instance.inventory.onScroll; set => instance.inventory.onScroll = value; }

		public static Action OnInventoryOpen { get => instance.inventory.onOpen; set => instance.inventory.onOpen = value; }
		public static Action OnInventoryClose { get => instance.inventory.onClose; set => instance.inventory.onClose = value; }

		public static bool AddItem(ItemType itemType, string itemName, int amount = 1)
		{
			return instance.inventory.AddItem(GameManager.DataBase.GetItem(itemType, itemName), amount);
		}

		public static void ActiveControl()
		{
			instance.hasControl = true;
			instance.onActiveControl.Invoke();
		}
		public static void DesactiveControl()
		{
			instance.hasControl = false;
			instance.onDesactiveControl.Invoke();
		}

		CharacterController characterController = null;
		Inventory inventory = null;

		bool hasControl = true;
		Action onActiveControl = () => { /*Debug.Log("OnActiveControl");*/ };
		Action onDesactiveControl = () => { /*Debug.Log("OnDesactiveControl");*/ };

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			inventory = GetComponent<Inventory>();

			instance = this;
		}
	}
}
