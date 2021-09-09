using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable IDE0044

namespace JTTF
{
    public class AnimationController : CustomNetworkBehaviour
    {
		[Header("DigAnimation")]
		[SerializeField] private float digAnimationDuration = 0.0f;
		[SerializeField] private float digAnimationMultiplier = 1.0f;

		[Header("CutAnimation")]
		[SerializeField] private float cutAnimationDuration = 0.0f;
		[SerializeField] private float cutAnimationMultiplier = 1.0f;

		[Header("MineAnimation")]
		[SerializeField] private float mineAnimationDuration = 0.0f;
		[SerializeField] private float mineAnimationMultiplier = 1.0f;

		[Header("PlantAnimation")]
		[SerializeField] private float plantAnimationDuration = 0.0f;
		[SerializeField] private float plantAnimationMultiplier = 1.0f;

		[Header("PlaceAnimation")]
		[SerializeField] private float placeAnimationDuration = 0.0f;
		[SerializeField] private float placeAnimationMultiplier = 1.0f;

		[Header("OpenAnimation")]
		[SerializeField] private float openAnimationDuration = 0.0f;
		[SerializeField] private float openAnimationMultiplier = 1.0f;

		[Header("PickAnimation")]
		[SerializeField] private float pickAnimationDuration = 0.0f;
		[SerializeField] private float pickAnimationMultiplier = 1.0f;

		private bool inAction = false;
		private Animator animator = null;
		private Player ownerPlayer = null;

		public Player OwnerPlayer => ownerPlayer;

		private float GetDesiredAnimationSpeed(float duration, float durationMax, float multiplier)
		{
			return (duration == 0 ? 1.0f : durationMax / duration) * multiplier;
		}

		private void OnStartToUseObject(ActionType actionType, float duration)
		{
			inAction = true;

			PlayActionAnimation(actionType, duration);
		}
		private void OnStopToUseObject(ActionType actionType, float duration)
		{
			inAction = false;

			StopActionAnimation(actionType);
		}

		private void OnStartToInteract(ActionType actionType, float duration)
		{
			inAction = true;

			PlayActionAnimation(actionType, duration);
		}
		private void OnStopToInteract(ActionType actionType, float duration)
		{
			inAction = false;

			StopActionAnimation(actionType);
		}

		private void PlayActionAnimation(ActionType actionType, float duration)
		{
			switch (actionType)
			{
				case ActionType.Dig:
					DigAnimation(true, GetDesiredAnimationSpeed(duration, digAnimationDuration, digAnimationMultiplier));
					break;

				case ActionType.Cut:
					CutAnimation(true, GetDesiredAnimationSpeed(duration, cutAnimationDuration, cutAnimationMultiplier));
					break;

				case ActionType.Mine:
					MineAnimation(true, GetDesiredAnimationSpeed(duration, mineAnimationDuration, mineAnimationMultiplier));
					break;

				case ActionType.Plant:
					PlantAnimation(true, GetDesiredAnimationSpeed(duration, plantAnimationDuration, plantAnimationMultiplier));
					break;

				case ActionType.Place:
					PlaceAnimation(true, GetDesiredAnimationSpeed(duration, placeAnimationDuration, placeAnimationMultiplier));
					break;

				case ActionType.Pick:
					PickAnimation(true, GetDesiredAnimationSpeed(duration, pickAnimationDuration, pickAnimationMultiplier));
					break;

				case ActionType.Open:
					OpenAnimation(true, GetDesiredAnimationSpeed(duration, openAnimationDuration, openAnimationMultiplier));
					break;

				case ActionType.None:
					break;
			}
		}
		private void StopActionAnimation(ActionType actionType)
		{
			switch (actionType)
			{
				case ActionType.Dig:
					DigAnimation(false);
					break;

				case ActionType.Cut:
					CutAnimation(false);
					break;

				case ActionType.Mine:
					MineAnimation(false);
					break;

				case ActionType.Plant:
					PlantAnimation(false);
					break;

				case ActionType.Place:
					PlaceAnimation(false);
					break;

				case ActionType.Pick:
					PickAnimation(false);
					break;

				case ActionType.Open:
					OpenAnimation(false);
					break;

				case ActionType.None:
					break;
			}
		}

		private void MovementAnimation(Vector3 direction, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			var localDirection = transform.worldToLocalMatrix * direction;
			animator.SetFloat("DirectionX", localDirection.x);
			animator.SetFloat("DirectionY", localDirection.z);
			animator.speed = speed;
		}
		private void DigAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			
			animator.SetBool("IsDig", value);
			animator.speed = speed;
		}
		private void CutAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsCut", value);
			animator.speed = speed;
		}
		private void MineAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsMine", value);
			animator.speed = speed;
		}
		private void PlantAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsPlant", value);
			animator.speed = speed;
		}
		private void PlaceAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsPlace", value);
			animator.speed = speed;
		}
		private void OpenAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsOpen", value);
			animator.speed = speed;
		}
		private void PickAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsPick", value);
			animator.speed = speed;
		}

		protected override void Awake()
		{
			base.Awake();

			ownerPlayer = GetComponent<Player>();
			animator = GetComponentInChildren<Animator>();
		}
		protected override void Start()
		{
			base.Start();
			
			if (!(this.IsClient && this.IsLocalPlayer))
			{
				this.enabled = false;
				return;
			}

			OwnerPlayer.UsableObjectController.onStartToUseObject += OnStartToUseObject;
			OwnerPlayer.UsableObjectController.onStopToUseObject += OnStopToUseObject;

			OwnerPlayer.InteractableController.onStartToInteract += OnStartToInteract;
			OwnerPlayer.InteractableController.onStopToInteract += OnStopToInteract;
		}
		protected override void Update()
		{
			base.Update();

			if (!inAction)
				MovementAnimation(OwnerPlayer.CharacterController.Direction);
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

			OwnerPlayer.UsableObjectController.onStartToUseObject -= OnStartToUseObject;
			OwnerPlayer.UsableObjectController.onStopToUseObject -= OnStopToUseObject;

			OwnerPlayer.InteractableController.onStartToInteract -= OnStartToInteract;
			OwnerPlayer.InteractableController.onStopToInteract -= OnStopToInteract;
		}
	}
}
