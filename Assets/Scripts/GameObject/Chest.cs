using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Chest : SimpleObject, IInteractable
	{
		public float Duration => duration;
		public float AnimationDuration => animationDuration;
		public float AnimationMultiplier => animationMultipler;

		[SerializeField] float duration = 0.0f;
		[SerializeField] float animationDuration = 0.0f;
		[SerializeField] float animationMultipler = 1.0f;

		[SerializeField] GameObject interactableImage = null;

		bool isOpen = false;

		Inventory inventory = null;
		Animator animator = null;

		public bool IsInteractable()
		{
			return inventory != null;
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

			if (isOpen)
				CloseChest();
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

		void PlayOpenChestAnim()
		{
			animator.SetBool("IsOpen", true);
		}
		void PlayCloseChestAnim()
		{
			animator.SetBool("IsOpen", false);
		}

		void OpenChest()
		{
			isOpen = true;
		}
		void CloseChest()
		{
			Debug.Log("CloseChest");
			isOpen = false;
			PlayCloseChestAnim();
		}

		private void Awake()
		{
			inventory = GetComponent<Inventory>();
			animator = GetComponent<Animator>();
		}
	}
}
