using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ActivableController : MonoBehaviour
    {
		[SerializeField] ProgressBar progressBar = null;

		bool isActive = false;
		float currentDuration = 0.0f;

		CharacterController characterController = null;
		AnimationController animationController = null;

		IUsable usableObject = null;

		void ActivateItem()
		{
			isActive = true;
			currentDuration = usableObject.Duration;

			progressBar.SetVisible(true);
			usableObject.PlayAnim(animationController);
			usableObject.Use();
		}
		void DesactivateItem()
		{
			usableObject.ApplyEffect();

			isActive = false;
			progressBar.SetVisible(false);
			usableObject.StopAnim(animationController);
			usableObject.Unuse();
		}
		void CancelItem()
		{
			if (!isActive) return;

			isActive = false;
			progressBar.SetVisible(false);
			usableObject.StopAnim(animationController);
			usableObject.Unuse();
		}

		void OnHandedObjectChange(GameObject handedObject)
		{
			if (handedObject != null)
				usableObject = handedObject.GetComponent<IUsable>();
		}

		void ItemInput()
		{
			if (Input.GetButtonDown("UseTool"))
				if (usableObject != null && characterController.IsIdle && usableObject.IsUsable())
					ActivateItem();
		}
		void UpdateItemDuration()
		{
			if (!isActive || usableObject == null) return;

			if (currentDuration <= 0.0f)
				DesactivateItem();

			currentDuration -= Time.deltaTime;
			progressBar.SetPercent( 1 - (currentDuration / usableObject.Duration));
		}

		private void Awake()
		{
			animationController = GetComponent<AnimationController>();
			characterController = GetComponent<CharacterController>();

			characterController.onMoveEnter += CancelItem;

			Player.OnHandedObjectChange += OnHandedObjectChange;
		}
		private void Update()
		{
			if (Player.HasControl)
			{
				ItemInput();

				UpdateItemDuration();
			}
		}
		private void OnDestroy()
		{
			characterController.onMoveEnter -= CancelItem;

			Player.OnHandedObjectChange -= OnHandedObjectChange;
		}
	}
}
