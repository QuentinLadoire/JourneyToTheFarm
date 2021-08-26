using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class ChestContainer : InteractableBehaviour
	{
		Animator animator = null;
		ChestInventoryController inventoryController = null;

		private void OnInventoryClose()
		{
			PlayCloseChestAnim();
		}

		private void PlayOpenChestAnim()
		{
			animator.SetBool("IsOpen", true);
		}
		private void PlayCloseChestAnim()
		{
			animator.SetBool("IsOpen", false);
		}

		protected override bool CheckIsInteractable()
		{
			return true;
		}

		protected override void Awake()
		{
			base.Awake();

			animator = GetComponent<Animator>();
			inventoryController = GetComponent<ChestInventoryController>();
		}
		private void Start()
		{
			inventoryController.onInventotyClose += OnInventoryClose;
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

			inventoryController.onInventotyClose -= OnInventoryClose;
		}

		public override void Select()
		{
			if (InteractableImage != null)
				InteractableImage.SetActive(true);

			InteractionText.SetText("Press E to Open");
			InteractionText.SetActive(true);
		}
		public override void Deselect()
		{
			if (InteractableImage != null)
				InteractableImage.SetActive(false);

			InteractionText.SetActive(false);
		}

		public override void StartToInteract()
		{
			PlayOpenChestAnim();
		}
		public override void Interact(Player player)
		{
			inventoryController.OpenInventory();
		}
	}
}
