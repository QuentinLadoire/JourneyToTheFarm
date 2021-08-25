using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class InteractableController : MonoBehaviour
    {
        public Player OwnerPlayer { get; private set; }

        public Action<ActionType, float> onStartToInteract = (ActionType actionType, float duration) => { /*Debug.Log("OnStartToInteract");*/ };
        public Action<ActionType, float> onInteract = (ActionType actionType, float duration) => { /*Debug.Log("OnInteract");*/ };
        public Action<ActionType, float> onStopToInteract = (ActionType actionType, float duration) => { /*Debug.Log("OnStopToInteract");*/ };

        [SerializeField] float checkRadius = 1.0f;

        bool inInteraction = false;
        float currentDuration = 0.0f;
        IInteractable interactableObject = null;
        PlayerProgressBar playerProgressBar = null;

        public IInteractable InteractableObject => interactableObject;

        void OnMoveEnter()
		{
            StopInteraction();
		}

        bool CanInteractObject()
		{
            return interactableObject != null && !interactableObject.Equals(null) && OwnerPlayer.CharacterController.IsIdle && interactableObject.IsInteractable();
        }
        void StartInteraction()
		{
            inInteraction = true;
            currentDuration = interactableObject.Duration;
            playerProgressBar.SetActive(true);
            interactableObject.StartToInteract();

            onStartToInteract.Invoke(interactableObject.ActionType, interactableObject.Duration);
        }
        void UpdateInteractionTime()
		{
            if (!inInteraction) return;

            if (currentDuration <= 0.0f)
                Interact();
            currentDuration -= Time.deltaTime;

            playerProgressBar.SetPercent(1 - (currentDuration / interactableObject.Duration));
        }
        void Interact()
		{
            StopInteraction();

            interactableObject.Interact(OwnerPlayer);
            onInteract.Invoke(interactableObject.ActionType, interactableObject.Duration);
        }
        void StopInteraction()
		{
            if (!inInteraction) return;

            inInteraction = false;
            currentDuration = 0.0f;
            playerProgressBar.SetActive(false);

            onStopToInteract.Invoke(interactableObject.ActionType, interactableObject.Duration);
		}

        void CheckHasNearestInteractableObject()
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

                if (interactableObject != null && interactableObject.IsInteractable())
                    interactableObject.Select();
            }
        }
        void ProcessInput()
		{
            if (Input.GetButton("Interact") && CanInteractObject())
                StartInteraction();
        }

		private void Awake()
		{
            OwnerPlayer = GetComponent<Player>();

            OwnerPlayer.CharacterController.onMoveEnter += OnMoveEnter;

            playerProgressBar = CanvasManager.GamePanel.PlayerPanel.PlayerProgressBar;
		}
		private void Update()
		{
			if (OwnerPlayer.CharacterController.HasControl)
			{
                CheckHasNearestInteractableObject();

                ProcessInput();

                UpdateInteractionTime();
			}
		}
		private void OnDestroy()
		{
            OwnerPlayer.CharacterController.onMoveEnter -= OnMoveEnter;
		}
	}
}
