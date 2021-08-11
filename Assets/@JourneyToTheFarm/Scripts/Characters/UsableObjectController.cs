using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class UsableObjectController : MonoBehaviour
    {
		public Player OwnerPlayer { get; private set; }

		public Action<ActionType, float> onStartToUseObject = (ActionType actionType, float duration) => { /*Debug.Log("OnStartToUseObejct");*/ };
		public Action<ActionType, float> onUseObject = (ActionType actionType, float duration) => { /*Debug.Log("OnUseObject");*/ };
		public Action<ActionType, float> onStopToUseObject = (ActionType actionType, float duration) => { /*Debug.Log("OnStopToUseObject");*/ };

		PlayerProgressBar playerProgressBar = null;

		bool inUse = false;
		IUsable usableObject = null;
		float currentDuration = 0.0f;

		void OnEquipedObjectChange(GameObject equipedObject)
		{
			if (usableObject != null)
			{
				StopToUseObject();
				usableObject = null;
			}

			if (equipedObject != null)
				usableObject = equipedObject.GetComponent<IUsable>();
		}
		void OnMoveEnter()
		{
			StopToUseObject();
		}

		bool CanUseObject()
		{
			return usableObject != null && !usableObject.Equals(null) && OwnerPlayer.CharacterController.IsIdle && usableObject.IsUsable;
		}
		void StartToUseObject()
		{
			inUse = true;
			currentDuration = usableObject.Duration;
			playerProgressBar.SetActive(true);

			onStartToUseObject.Invoke(usableObject.ActionType, usableObject.Duration);
		}
		void UpdateUseTime()
		{
			if (!inUse) return;

			if (currentDuration <= 0.0f)
				UseObject();
			currentDuration -= Time.deltaTime;

			playerProgressBar.SetPercent(1 - (currentDuration / usableObject.Duration));
		}
		void UseObject()
		{
			StopToUseObject();

			usableObject.Use();
			onUseObject.Invoke(usableObject.ActionType, usableObject.Duration);
		}
		void StopToUseObject()
		{
			if (inUse)
			{
				inUse = false;
				currentDuration = 0.0f;
				playerProgressBar.SetActive(false);

				onStopToUseObject.Invoke(usableObject.ActionType, usableObject.Duration);
			}
		}

		void ProcessInput()
		{
			if (Input.GetButtonDown("UseObject") && CanUseObject())
				StartToUseObject();
		}

		private void Awake()
		{
			OwnerPlayer = GetComponent<Player>();

			OwnerPlayer.EquipableController.onEquipedObjectChange += OnEquipedObjectChange;
			OwnerPlayer.CharacterController.onMoveEnter += OnMoveEnter;

			playerProgressBar = CanvasManager.GamePanel.PlayerPanel.PlayerProgressBar;
		}
		private void Update()
		{
			if (OwnerPlayer.CharacterController.HasControl)
			{
				ProcessInput();

				UpdateUseTime();
			}
		}
		private void OnDestroy()
		{
			OwnerPlayer.EquipableController.onEquipedObjectChange -= OnEquipedObjectChange;
			OwnerPlayer.CharacterController.onMoveEnter -= OnMoveEnter;
		}
	}
}
