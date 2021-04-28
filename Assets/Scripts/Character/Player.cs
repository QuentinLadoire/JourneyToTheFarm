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

		public static RecipeDataBase CraftingRecipe { get => instance.craftingController.RecipeDataBase; }

		//CharacterController
		public static Action<Vector3> OnHasMoved { get => instance.characterController.onHasMoved; set => instance.characterController.onHasMoved = value; }

		//Inventory
		public static Action<int, string, int, ItemType> OnAddItem { get => instance.inventory.onAddItem; set => instance.inventory.onAddItem = value; }
		public static Action<int, string, int, ItemType> OnRemoveItem { get => instance.inventory.onRemoveItem; set => instance.inventory.onRemoveItem = value; }

		//InventoryController
		public static Action OnInventoryOpen { get => instance.inventoryController.onOpen; set => instance.inventoryController.onOpen = value; }
		public static Action OnInventoryClose { get => instance.inventoryController.onClose; set => instance.inventoryController.onClose = value; }
		public static Action<int, string, ItemType> OnScroll { get => instance.inventoryController.onScroll; set => instance.inventoryController.onScroll = value; }

		//CraftingController
		public static Action OnCraftingOpen { get => instance.craftingController.onOpen; set => instance.craftingController.onOpen = value; }
		public static Action OnCraftingClose { get => instance.craftingController.onClose; set => instance.craftingController.onClose = value; }

		public static Action<Recipe> OnStartCraft { get => instance.craftingController.onStartCraft; set => instance.craftingController.onStartCraft = value; }
		public static Action OnCancelCraft { get => instance.craftingController.onCancelCraft; set => instance.craftingController.onCancelCraft = value; }
		public static Action OnEndCraft { get => instance.craftingController.onEndCraft; set => instance.craftingController.onEndCraft = value; }
		public static Action<float> OnCraft { get => instance.craftingController.onCraft; set => instance.craftingController.onCraft = value; }


		public static int HowManyItem(string name)
		{
			return instance.inventory.HowManyItem(name);
		}
		public static bool HasItem(string name, int amount)
		{
			return instance.inventory.HasItem(name, amount);
		}
		public static int AddItem(string name, int amount, ItemType itemType)
		{
			return instance.inventory.AddItem(name, amount, itemType);
		}
		public static void RemoveItem(string name, int amount)
		{
			instance.inventory.RemoveItem(name, amount);
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
		InventoryController inventoryController = null;
		CraftingController craftingController = null;
		Inventory inventory = null;

		bool hasControl = true;
		Action onActiveControl = () => { /*Debug.Log("OnActiveControl");*/ };
		Action onDesactiveControl = () => { /*Debug.Log("OnDesactiveControl");*/ };

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			inventoryController = GetComponent<InventoryController>();
			craftingController = GetComponent<CraftingController>();
			inventory = GetComponent<Inventory>();

			instance = this;
		}
	}
}
