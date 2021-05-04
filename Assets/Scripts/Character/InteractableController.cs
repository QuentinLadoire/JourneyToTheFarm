using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class InteractableController : MonoBehaviour
    {
        [SerializeField] ProgressBar progressBar = null;

        [SerializeField] float checkRadius = 1.0f;

        AnimationController animationController = null;

        IInteractable interactableObject = null;

        bool isInteracted = false;
        float currentDuration = 0.0f;

        void OnMoveEnter()
		{
            CancelInteraction();
		}

        void StartInteraction()
		{
            if (isInteracted) return;

            isInteracted = true;

            currentDuration = interactableObject.Duration;

            progressBar.SetVisible(true);

            interactableObject.PlayAnim(animationController);
        }
        void EndInteraction()
		{
            if (!isInteracted) return;

            isInteracted = false;

            currentDuration = 0.0f;

            progressBar.SetVisible(false);

            interactableObject.StopAnim(animationController);

            interactableObject.Interact();
		}
        void CancelInteraction()
		{
            if (!isInteracted) return;

            isInteracted = false;

            currentDuration = 0.0f;

            progressBar.SetVisible(false);

            interactableObject.StopAnim(animationController);
		}

        void CheckHasNearestInteractableObject()
		{
            if (isInteracted) return;

            IInteractable nearestInteractable = null;
            float nearestSqrtDistance = 0.0f;

            var colliders = Physics.OverlapSphere(transform.position, checkRadius);
            foreach (var collider in colliders)
			{
                var interactable = collider.GetComponentInParent<IInteractable>();
                if (interactable != null && interactable.IsInteractable())
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

                if (interactableObject != null)
                    interactableObject.Select();
            }
        }
        void ProcessInput()
		{
            if (Input.GetButton("Interact"))
                if (interactableObject != null && Player.IsIdle && interactableObject.IsInteractable())
                    StartInteraction();
        }
        void UpdateDuration()
		{
            if (!isInteracted || interactableObject == null) return;

            if (currentDuration <= 0.0f)
                EndInteraction();

            currentDuration -= Time.deltaTime;

            progressBar.SetPercent(1.0f - (currentDuration / interactableObject.Duration));
		}

		private void Awake()
		{
            animationController = GetComponent<AnimationController>();

            Player.OnMoveEnter += OnMoveEnter;
		}
		private void Update()
		{
			if (Player.HasControl)
			{
                CheckHasNearestInteractableObject();

                ProcessInput();

                UpdateDuration();
			}
		}
		private void OnDestroy()
		{
            Player.OnMoveEnter -= OnMoveEnter;
		}
	}
}
