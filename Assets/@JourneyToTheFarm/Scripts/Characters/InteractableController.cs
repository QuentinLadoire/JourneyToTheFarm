using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class InteractableController : CustomNetworkBehaviour
    {
        [SerializeField] private float checkRadius = 1.0f;

        private Player ownerPlayer = null;
        private bool inInteraction = false;
        private float currentDuration = 0.0f;
        private IInteractable interactableObject = null;
        private PlayerProgressBar playerProgressBar = null;

        public Player OwnerPlayer => ownerPlayer;
        public IInteractable InteractableObject => interactableObject;

        public Action<ActionType, float> onStartToInteract = (ActionType actionType, float duration) => { /*Debug.Log("OnStartToInteract");*/ };
        public Action<ActionType, float> onInteract = (ActionType actionType, float duration) => { /*Debug.Log("OnInteract");*/ };
        public Action<ActionType, float> onStopToInteract = (ActionType actionType, float duration) => { /*Debug.Log("OnStopToInteract");*/ };

        private void OnMoveEnter()
		{
            StopInteraction();
		}

        private bool CanInteractObject()
		{
            return interactableObject != null && !interactableObject.Equals(null) && OwnerPlayer.CharacterController.IsIdle && interactableObject.IsInteractable;
        }
        private void StartInteraction()
		{
            inInteraction = true;
            currentDuration = interactableObject.ActionDuration;
            playerProgressBar.SetActive(true);
            interactableObject.StartToInteract();

            onStartToInteract.Invoke(interactableObject.ActionType, interactableObject.ActionDuration);
        }
        private void UpdateInteractionTime()
		{
            if (!inInteraction) return;

            if (currentDuration <= 0.0f)
                Interact();
            currentDuration -= Time.deltaTime;

            playerProgressBar.SetPercent(1 - (currentDuration / interactableObject.ActionDuration));
        }
        private void Interact()
		{
            StopInteraction();

            interactableObject.Interact(OwnerPlayer);
            onInteract.Invoke(interactableObject.ActionType, interactableObject.ActionDuration);
        }
        private void StopInteraction()
		{
            if (!inInteraction) return;

            inInteraction = false;
            currentDuration = 0.0f;
            playerProgressBar.SetActive(false);

            onStopToInteract.Invoke(interactableObject.ActionType, interactableObject.ActionDuration);
		}

        private void CheckHasNearestInteractableObject()
		{
            if (inInteraction) return;

            IInteractable nearestInteractable = null;
            float nearestSqrtDistance = 0.0f;

            var colliders = Physics.OverlapSphere(transform.position, checkRadius);
            foreach (var collider in colliders)
			{
                var interactable = collider.GetComponentInParent<IInteractable>();
                if (interactable != null)
				{
                    var sqrtDistance = (collider.transform.position - transform.position).sqrMagnitude;
                    if (nearestInteractable == null || sqrtDistance < nearestSqrtDistance)
					{
                        nearestInteractable = interactable;
                        nearestSqrtDistance = sqrtDistance;
					}
				}
			}

            if (interactableObject != nearestInteractable)
            {
                if (interactableObject != null)
                    interactableObject.Deselect();

                interactableObject = nearestInteractable;

                if (interactableObject != null && interactableObject.IsInteractable)
                    interactableObject.Select();
            }
        }
        private void ProcessInput()
		{
            if (Input.GetButton("Interact") && CanInteractObject())
                StartInteraction();
        }

		protected override void Awake()
		{
            base.Awake();

            ownerPlayer = GetComponent<Player>();
		}
        protected override void Start()
		{
            base.Start();

            if (!(this.IsClient && this.IsLocalPlayer))
			{
                this.enabled = false;
                return;
			}

            playerProgressBar = CanvasManager.GamePanel.PlayerPanel.PlayerProgressBar;
            OwnerPlayer.CharacterController.onMoveEnter += OnMoveEnter;
        }
		protected override void Update()
		{
            base.Update();

			if (OwnerPlayer.CharacterController.HasControl)
			{
                CheckHasNearestInteractableObject();

                ProcessInput();

                UpdateInteractionTime();
			}
		}
		protected override void OnDestroy()
		{
            base.OnDestroy();

            OwnerPlayer.CharacterController.onMoveEnter -= OnMoveEnter;
		}
	}
}
