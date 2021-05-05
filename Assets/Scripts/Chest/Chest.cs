using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Chest : SimpleObject, IInteractable
	{
		public static Action<ChestInventoryController> OnOpenInventory { get => ChestInventoryController.onOpenInventory; set => ChestInventoryController.onOpenInventory = value; }
		public static Action<ChestInventoryController> OnCloseInventory { get => ChestInventoryController.onCloseInventory; set => ChestInventoryController.onCloseInventory = value; }

		public Inventory Inventory => inventoryController.Inventory;

		public float Duration => duration;
		public float AnimationDuration => animationDuration;
		public float AnimationMultiplier => animationMultipler;

		[SerializeField] float duration = 0.0f;
		[SerializeField] float animationDuration = 0.0f;
		[SerializeField] float animationMultipler = 1.0f;
		[SerializeField] GameObject interactableImage = null;

		ChestInventoryController inventoryController = null;
		Animator animator = null;

		void OnCloseChest(ChestInventoryController chestInventoryController)
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
		public void Interact()
		{
			inventoryController.OpenInventory();
		}

		public void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterOpening(true, animationController.GetDesiredAnimationSpeed(duration, animationDuration, animationMultipler));

			PlayOpenChestAnim();
		}
		public void StopAnim(AnimationController animationController)
		{
			animationController.CharacterOpening(false);
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
