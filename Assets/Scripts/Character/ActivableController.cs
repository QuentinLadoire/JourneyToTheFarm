using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ActivableController : MonoBehaviour
    {
		[SerializeField] ProgressBar progressBar = null;

        ActivableObject activableObject = null;

		bool isActive = false;
		float currentDuration = 0.0f;

		CharacterController characterController = null;
		AnimationController animationController = null;
		TransportableController transportableController = null;

		void ActivateItem()
		{
			isActive = true;
			currentDuration = activableObject.duration;

			progressBar.SetVisible(true);
			activableObject.PlayAnim(animationController);
			activableObject.Activate();
		}
		void DesactivateItem()
		{
			activableObject.ApplyEffect();

			isActive = false;
			progressBar.SetVisible(false);
			activableObject.StopAnim(animationController);
			activableObject.Desactivate();
		}
		void CancelItem()
		{
			if (!isActive) return;

			isActive = false;
			progressBar.SetVisible(false);
			activableObject.StopAnim(animationController);
			activableObject.Desactivate();
		}

		void OnTransportableObjectChange(TransportableObject transportableObject)
		{
			if (activableObject != null)
				CancelItem();

			if (transportableObject != null)
				activableObject = transportableObject.GetComponent<ActivableObject>();
		}

		void ItemInput()
		{
			if (Input.GetButtonDown("UseTool"))
				if (activableObject != null && characterController.IsIdle && activableObject.IsActivable())
					ActivateItem();
		}
		void UpdateItemDuration()
		{
			if (!isActive || activableObject == null) return;

			if (currentDuration <= 0.0f)
				DesactivateItem();

			currentDuration -= Time.deltaTime;
			progressBar.SetPercent( 1 - (currentDuration / activableObject.duration));
		}

		private void Awake()
		{
			animationController = GetComponent<AnimationController>();
			characterController = GetComponent<CharacterController>();
			transportableController = GetComponent<TransportableController>();

			characterController.onMoveEnter += CancelItem;
			transportableController.onTransportableObjectChange += OnTransportableObjectChange;
		}
		private void Update()
		{
			ItemInput();

			UpdateItemDuration();
		}
		private void OnDestroy()
		{
			characterController.onMoveEnter -= CancelItem;
			transportableController.onTransportableObjectChange -= OnTransportableObjectChange;
		}
	}
}
