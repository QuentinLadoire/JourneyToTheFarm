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
		public static Vector3 RoundForward { get => instance.transform.forward.RoundToInt(); }

		public static bool HasControl { get => instance.hasControl; }
		public static Action OnActiveControl { get => instance.onActiveControl; set => instance.onActiveControl = value; }
		public static Action OnDesactiveControl { get => instance.onDesactiveControl; set => instance.onDesactiveControl = value; }

		public static RecipeDataBase CraftingRecipe { get => instance.craftingController.RecipeDataBase; }

		//CharacterController
		public static Action OnMoveEnter { get => instance.characterController.onMoveEnter; set => instance.characterController.onMoveEnter = value; }
		public static Action OnMoveExit { get => instance.characterController.onMoveExit; set => instance.characterController.onMoveExit = value; }
		public static Action OnHasMoved { get => instance.characterController.onHasMoved; set => instance.characterController.onHasMoved = value; }

		public static bool IsIdle => instance.characterController.IsIdle;

		//HandController
		public static Action<GameObject> OnHandedObjectChange { get => instance.handController.onHandedObjectChange; set => instance.handController.onHandedObjectChange = value; }

		//InventoryController
		public static InventoryController InventoryController { get => instance.inventoryController; }
		public static Action<int, ItemInfo> OnAddItem { get => instance.inventoryController.onAddItem; set => instance.inventoryController.onAddItem = value; }
		public static Action<int, ItemInfo> OnRemoveItem { get => instance.inventoryController.onRemoveItem; set => instance.inventoryController.onRemoveItem = value; }
		public static Action<InventoryController> OnInventoryOpen { get => instance.inventoryController.onOpenInventory; set => instance.inventoryController.onOpenInventory = value; }
		public static Action<InventoryController> OnInventoryClose { get => instance.inventoryController.onCloseInventory; set => instance.inventoryController.onCloseInventory = value; }
		public static Action<int, string, ItemType, int> OnScroll { get => instance.inventoryController.onScroll; set => instance.inventoryController.onScroll = value; }

		//CraftingController
		public static Action OnCraftingOpen { get => instance.craftingController.onOpen; set => instance.craftingController.onOpen = value; }
		public static Action OnCraftingClose { get => instance.craftingController.onClose; set => instance.craftingController.onClose = value; }

		public static Action<Recipe> OnStartCraft { get => instance.craftingController.onStartCraft; set => instance.craftingController.onStartCraft = value; }
		public static Action OnCancelCraft { get => instance.craftingController.onCancelCraft; set => instance.craftingController.onCancelCraft = value; }
		public static Action OnEndCraft { get => instance.craftingController.onEndCraft; set => instance.craftingController.onEndCraft = value; }
		public static Action<float> OnCraft { get => instance.craftingController.onCraft; set => instance.craftingController.onCraft = value; }

		public static void AddItem(ItemInfo itemInfo)
		{
			instance.inventoryController.AddItem(itemInfo);
		}
		public static void RemoveItem(ItemInfo itemInfo)
		{
			instance.inventoryController.RemoveItem(itemInfo);
		}

		public static int HowManyItem(string name)
		{
			return instance.inventoryController.HowManyItem(name);
		}
		public static bool HasItemAmount(string name, int amount)
		{
			return instance.inventoryController.HasItemAmount(name, amount);
		}

		public static void Craft(Recipe recipe)
		{
			instance.craftingController.StartCraft(recipe);
		}
		public static void CancelCraft()
		{
			instance.craftingController.CancelCraft();
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
		HandController handController = null;
		PlayerInventoryController inventoryController = null;
		CraftingController craftingController = null;

		bool hasControl = true;
		Action onActiveControl = () => { /*Debug.Log("OnActiveControl");*/ };
		Action onDesactiveControl = () => { /*Debug.Log("OnDesactiveControl");*/ };

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			handController = GetComponent<HandController>();
			inventoryController = GetComponent<PlayerInventoryController>();
			craftingController = GetComponent<CraftingController>();

			instance = this;
		}
	}
}
