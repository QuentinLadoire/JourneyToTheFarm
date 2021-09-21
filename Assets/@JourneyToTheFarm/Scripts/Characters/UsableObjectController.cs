using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.UI;
using JTTF.Enum;
using JTTF.Behaviour;
using JTTF.Interface;

namespace JTTF.Character
{
    public class UsableObjectController : CustomNetworkBehaviour
    {
		private bool inUse = false;
		private Player ownerPlayer = null;
		private IUsable usableObject = null;
		private float currentDuration = 0.0f;
		private PlayerProgressBar playerProgressBar = null;

		public Player OwnerPlayer => ownerPlayer;

		public Action<ActionType, float> onStartToUseObject = (ActionType actionType, float duration) => { /*Debug.Log("OnStartToUseObejct");*/ };
		public Action<ActionType, float> onUseObject = (ActionType actionType, float duration) => { /*Debug.Log("OnUseObject");*/ };
		public Action<ActionType, float> onStopToUseObject = (ActionType actionType, float duration) => { /*Debug.Log("OnStopToUseObject");*/ };

		private void OnEquipedObjectChange(GameObject equipedObject)
		{
			if (usableObject != null)
			{
				StopToUseObject();
				usableObject = null;
			}

			if (equipedObject != null)
				usableObject = equipedObject.GetComponent<IUsable>();
		}
		private void OnMoveEnter()
		{
			StopToUseObject();
		}

		private bool CanUseObject()
		{
			return usableObject != null && !usableObject.Equals(null) && OwnerPlayer.MovementController.IsIdle && usableObject.IsUsable;
		}
		private void StartToUseObject()
		{
			inUse = true;
			currentDuration = usableObject.ActionDuration;
			playerProgressBar.SetActive(true);

			onStartToUseObject.Invoke(usableObject.ActionType, usableObject.ActionDuration);
		}
		private void UpdateUseTime()
		{
			if (!inUse) return;

			playerProgressBar.SetPercent(1 - (currentDuration / usableObject.ActionDuration));

			if (currentDuration <= 0.0f)
				UseObject();
			currentDuration -= Time.deltaTime;
		}
		private void UseObject()
		{
			StopToUseObject();

			var actionType = usableObject.ActionType;
			var duration = usableObject.ActionDuration;

			usableObject.Use();
			onUseObject.Invoke(actionType, duration);
		}
		private void StopToUseObject()
		{
			if (inUse)
			{
				inUse = false;
				currentDuration = 0.0f;
				playerProgressBar.SetActive(false);

				onStopToUseObject.Invoke(usableObject.ActionType, usableObject.ActionDuration);
			}
		}

		private void ProcessInput()
		{
			if (Input.GetButtonDown("UseObject") && CanUseObject())
				StartToUseObject();
		}

		protected override void Awake()
		{
			base.Awake();

			ownerPlayer = GetComponent<Player>();
		}
		public override void NetworkStart()
		{
			base.NetworkStart();

			if (!(this.IsClient && this.IsLocalPlayer))
			{
				this.enabled = false;
				return;
			}
		}
		protected override void Start()
		{
			base.Start();

			OwnerPlayer.EquipableController.onEquipedObjectChange += OnEquipedObjectChange;
			OwnerPlayer.MovementController.onMoveEnter += OnMoveEnter;

			playerProgressBar = CanvasManager.GamePanel.PlayerPanel.PlayerProgressBar;
		}
		protected override void Update()
		{
			base.Update();

			if (OwnerPlayer.MovementController.HasControl)
			{
				ProcessInput();

				UpdateUseTime();
			}
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

			OwnerPlayer.EquipableController.onEquipedObjectChange -= OnEquipedObjectChange;
			OwnerPlayer.MovementController.onMoveEnter -= OnMoveEnter;
		}
	}
}
