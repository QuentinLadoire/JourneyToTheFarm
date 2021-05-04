using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Chest : SimpleObject, IInteractable
	{
		public static Action<Chest> OnOpenChestInventory { get; set; } = (Chest chest) => { /*Debug.Log("OnOpenChestInventory");*/ };
		public static Action<Chest> OnCloseChestInventory { get; set; } = (Chest chest) => { /*Debug.Log("OnOpenChestInventory");*/ };

		public Inventory Inventory => inventory;

		public float Duration => duration;
		public float AnimationDuration => animationDuration;
		public float AnimationMultiplier => animationMultipler;

		[SerializeField] float duration = 0.0f;
		[SerializeField] float animationDuration = 0.0f;
		[SerializeField] float animationMultipler = 1.0f;

		[SerializeField] GameObject interactableImage = null;

		Inventory inventory = null;
		Animator animator = null;

		void PlayOpenChestAnim()
		{
			animator.SetBool("IsOpen", true);
		}
		void PlayCloseChestAnim()
		{
			animator.SetBool("IsOpen", false);
		}

		public void OpenChest()
		{
			OnOpenChestInventory.Invoke(this);
		}
		public void CloseChest()
		{
			PlayCloseChestAnim();

			OnCloseChestInventory.Invoke(this);
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
			return inventory != null;
		}
		public void Interact()
		{
			OpenChest();
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

		private void Awake()
		{
			inventory = GetComponent<Inventory>();
			animator = GetComponent<Animator>();
		}
	}
}
