using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Chest : CustomBehaviour, IInteractable
	{
		public float Duration => duration;
		public ActionType ActionType => ActionType.Open;

		[Header("Chest Parameters")]
		public float duration = 0.0f;
		public GameObject interactableImage = null;

		Animator animator = null;

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
			
		}

		protected override void Awake()
		{
			base.Awake();

			animator = GetComponent<Animator>();
		}
	}
}
