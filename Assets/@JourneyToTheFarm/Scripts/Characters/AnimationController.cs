using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class AnimationController : MonoBehaviour
    {
		public Player OwnerPlayer { get; private set; }

		[Header("DigAnimation")]
		[SerializeField] float digAnimationDuration = 0.0f;
		[SerializeField] float digAnimationMultiplier = 1.0f;

		[Header("CutAnimation")]
		[SerializeField] float cutAnimationDuration = 0.0f;
		[SerializeField] float cutAnimationMultiplier = 1.0f;

		[Header("MineAnimation")]
		[SerializeField] float mineAnimationDuration = 0.0f;
		[SerializeField] float mineAnimationMultiplier = 1.0f;

		[Header("PlantAnimation")]
		[SerializeField] float plantAnimationDuration = 0.0f;
		[SerializeField] float plantAnimationMultiplier = 1.0f;

		[Header("PlaceAnimation")]
		[SerializeField] float placeAnimationDuration = 0.0f;
		[SerializeField] float placeAnimationMultiplier = 1.0f;

		[Header("OpenAnimation")]
		[SerializeField] float openAnimationDuration = 0.0f;
		[SerializeField] float openAnimationMultiplier = 1.0f;

		[Header("PickAnimation")]
		[SerializeField] float pickAnimationDuration = 0.0f;
		[SerializeField] float pickAnimationMultiplier = 1.0f;

		Animator animator = null;
		bool inAction = false;

		float GetDesiredAnimationSpeed(float duration, float durationMax, float multiplier)
		{
			return (duration == 0 ? 1.0f : durationMax / duration) * multiplier;
		}

		void OnStartToUseObject(ActionType actionType, float duration)
		{
			inAction = true;

			PlayActionAnimation(actionType, duration);
		}
		void OnStopToUseObject(ActionType actionType, float duration)
		{
			inAction = false;

			StopActionAnimation(actionType);
		}

		void OnStartToInteract(ActionType actionType, float duration)
		{
			inAction = true;

			PlayActionAnimation(actionType, duration);
		}
		void OnStopToInteract(ActionType actionType, float duration)
		{
			inAction = false;

			StopActionAnimation(actionType);
		}

		void PlayActionAnimation(ActionType actionType, float duration)
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
		void StopActionAnimation(ActionType actionType)
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

		void MovementAnimation(Vector3 direction, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			var localDirection = transform.worldToLocalMatrix * direction;
			animator.SetFloat("DirectionX", localDirection.x);
			animator.SetFloat("DirectionY", localDirection.z);
			animator.speed = speed;
		}
		void DigAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			
			animator.SetBool("IsDig", value);
			animator.speed = speed;
		}
		void CutAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsCut", value);
			animator.speed = speed;
		}
		void MineAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsMine", value);
			animator.speed = speed;
		}
		void PlantAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsPlant", value);
			animator.speed = speed;
		}
		void PlaceAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsPlace", value);
			animator.speed = speed;
		}
		void OpenAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsOpen", value);
			animator.speed = speed;
		}
		void PickAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsPick", value);
			animator.speed = speed;
		}

		private void Awake()
		{
			OwnerPlayer = GetComponent<Player>();
			animator = GetComponentInChildren<Animator>();
		}
		private void Start()
		{
			OwnerPlayer.UsableObjectController.onStartToUseObject += OnStartToUseObject;
			OwnerPlayer.UsableObjectController.onStopToUseObject += OnStopToUseObject;

			OwnerPlayer.InteractableController.onStartToInteract += OnStartToInteract;
			OwnerPlayer.InteractableController.onStopToInteract += OnStopToInteract;
		}
		private void Update()
		{
			if (!inAction)
				MovementAnimation(OwnerPlayer.CharacterController.Direction);
		}
		private void OnDestroy()
		{
			OwnerPlayer.UsableObjectController.onStartToUseObject -= OnStartToUseObject;
			OwnerPlayer.UsableObjectController.onStopToUseObject -= OnStopToUseObject;

			OwnerPlayer.InteractableController.onStartToInteract -= OnStartToInteract;
			OwnerPlayer.InteractableController.onStopToInteract -= OnStopToInteract;
		}
	}
}
