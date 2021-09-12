using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;
using JTTF.Character;
using JTTF.Inventory;

namespace JTTF.Gameplay
{
	public class ChestContainer : InteractableBehaviour
	{
		private Animator animator = null;
		private ChestInventoryController inventoryController = null;

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
		protected override void Start()
		{
			base.Start();

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
