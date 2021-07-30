using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Chest : CustomBehaviour, IInteractable
	{
		public static Action<InventoryController> OnOpenInventory { get => ChestInventoryController.onOpenInventory; set => ChestInventoryController.onOpenInventory = value; }
		public static Action<InventoryController> OnCloseInventory { get => ChestInventoryController.onCloseInventory; set => ChestInventoryController.onCloseInventory = value; }

		public float Duration => duration;
		public ActionType ActionType => ActionType.Open;

		[Header("Chest Parameters")]
		public float duration = 0.0f;
		public GameObject interactableImage = null;

		ChestInventoryController inventoryController = null;
		Animator animator = null;

		void OnCloseChest(InventoryController controller)
		{
			PlayCloseChestAnim();
		}
		void PlayOpenChestAnim()
		{
			animator.SetBool("IsOpen", true);
		}
		void PlayCloseChestAnim()
		{
			animator.SetBool("IsOpen", false);
		}

		public void Select()
		{
			if (interactableImage != null)
				interactableImage.SetActive(true);
		}
		public void Deselect()
		{
			if (interactableImage != null)
				interactableImage.SetActive(false);
		}

		public bool IsInteractable()
		{
			return true;
		}
		public void StartToInteract()
		{
			PlayOpenChestAnim();
		}
		public void Interact()
		{
			inventoryController.OpenInventory();
		}

		protected override void Awake()
		{
			base.Awake();

			inventoryController = GetComponent<ChestInventoryController>();
			animator = GetComponent<Animator>();

			OnCloseInventory += OnCloseChest;
		}
		private void OnDestroy()
		{
			OnCloseInventory -= OnCloseChest;
		}
	}
}
