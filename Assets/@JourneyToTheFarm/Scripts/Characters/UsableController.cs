using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class UsableController : MonoBehaviour
    {
		[SerializeField] FarmerProgressBar progressBar = null;

		bool isActive = false;
		float currentDuration = 0.0f;

		CharacterController characterController = null;
		AnimationController animationController = null;

		IUsable usableObject = null;

		void UseItem()
		{
			isActive = true;
			currentDuration = usableObject.Duration;

			progressBar.SetActive(true);
			usableObject.PlayAnim(animationController);
		}
		void UnuseItem()
		{
			usableObject.Use();

			isActive = false;
			progressBar.SetActive(false);
			usableObject.StopAnim(animationController);
		}
		void CancelItem()
		{
			if (!isActive) return;

			isActive = false;
			progressBar.SetActive(false);
			usableObject.StopAnim(animationController);
		}

		void OnHandedObjectChange(GameObject handedObject)
		{
			if (usableObject != null)
			{
				CancelItem();
				usableObject = null;
			}

			if (handedObject != null)
				usableObject = handedObject.GetComponent<IUsable>();
		}

		void ProcessInput()
		{
			if (Input.GetButtonDown("UseTool"))
				if (usableObject != null && !usableObject.Equals(null) && characterController.IsIdle && usableObject.IsUsable())
					UseItem();
		}
		void UpdateItemDuration()
		{
			if (!isActive || usableObject == null) return;

			if (currentDuration <= 0.0f)
				UnuseItem();

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
				ProcessInput();

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
