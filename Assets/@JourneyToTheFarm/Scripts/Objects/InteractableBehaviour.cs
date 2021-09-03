using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class InteractableBehaviour : CustomBehaviour, IInteractable
	{
		[Header("InteractableBehaviour Settings")]
		[SerializeField] float actionDuration = 0.0f;
		[SerializeField] ActionType actionType = ActionType.None;
		[SerializeField] GameObject interactableImage = null;

		private bool isInteractable = false;
		private PlayerInteractionText interactionText = null;

		public bool IsInteractable => isInteractable;
		public float ActionDuration => actionDuration;
		public ActionType ActionType => actionType;
		public GameObject InteractableImage => interactableImage;
		public PlayerInteractionText InteractionText => interactionText;

		protected override void Awake()
		{
			base.Awake();

			interactionText = CanvasManager.GamePanel.PlayerPanel.PlayerInteractionText;
		}
		protected override void Update()
		{
			base.Update();

			isInteractable = CheckIsInteractable();
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (interactionText != null)
				interactionText.SetActive(false);
		}

		protected virtual bool CheckIsInteractable()
		{
			return false;
		}
		
		public virtual void Select()
		{
			interactableImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		}
		public virtual void Deselect()
		{
			interactableImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}

		public virtual void StartToInteract()
		{
			//nothing
		}
		public virtual void Interact(Player player)
		{
			Debug.LogWarning("The Mehtod Interact of " + this.GetType().ToString() + ", is not Implemented.");
		}
	}
}
