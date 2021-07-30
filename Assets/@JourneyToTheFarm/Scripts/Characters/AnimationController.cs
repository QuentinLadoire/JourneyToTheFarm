using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class AnimationController : MonoBehaviour
    {
		[Header("DigAnimation")]
		public float digAnimationDuration = 0.0f;
		public float digAnimationMultiplier = 1.0f;

		[Header("CutAnimation")]
		public float cutAnimationDuration = 0.0f;
		public float cutAnimationMultiplier = 1.0f;

		[Header("MineAnimation")]
		public float mineAnimationDuration = 0.0f;
		public float mineAnimationMultiplier = 1.0f;

		[Header("PlantAnimation")]
		public float plantAnimationDuration = 0.0f;
		public float plantAnimationMultiplier = 1.0f;

		[Header("PlaceAnimation")]
		public float placeAnimationDuration = 0.0f;
		public float placeAnimationMultiplier = 1.0f;

		[Header("OpenAnimation")]
		public float openAnimationDuration = 0.0f;
		public float openAnimationMultiplier = 1.0f;

		[Header("PickAnimation")]
		public float pickAnimationDuration = 0.0f;
		public float pickAnimationMultiplier = 1.0f;

		Animator animator = null;

		bool inAction = false;

		private void Awake()
		{
			animator = GetComponentInChildren<Animator>();
		}
		private void Start()
		{
			Player.OnStartToUseObject += OnStartToUseObject;
			Player.OnStopToUseObject += OnStopToUseObject;
		}
		private void Update()
		{
			if (!inAction)
				MovementAnimation(Player.Direction);
		}
		private void OnDestroy()
		{
			Player.OnStartToUseObject -= OnStartToUseObject;
			Player.OnStopToUseObject -= OnStopToUseObject;
		}

		public float GetDesiredAnimationSpeed(float duration, float durationMax, float multiplier)
		{
			return (duration == 0 ? 1.0f : durationMax / duration) * multiplier;
		}

		void OnStartToUseObject(ActionType actionType, float duration)
		{
			inAction = true;

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

				case ActionType.None:
					break;
			}
		}
		void OnStopToUseObject(ActionType actionType, float duration)
		{
			inAction = false;

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

				case ActionType.None:
					break;
			}
		}

		public void MovementAnimation(Vector3 direction, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			var localDirection = transform.worldToLocalMatrix * direction;
			animator.SetFloat("DirectionX", localDirection.x);
			animator.SetFloat("DirectionY", localDirection.z);
			animator.speed = speed;
		}
		public void DigAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			
			animator.SetBool("IsDig", value);
			animator.speed = speed;
		}
		public void CutAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsCut", value);
			animator.speed = speed;
		}
		public void MineAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsMine", value);
			animator.speed = speed;
		}
		public void PlantAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsPlant", value);
			animator.speed = speed;
		}
		public void PlaceAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsPlace", value);
			animator.speed = speed;
		}
		public void OpenAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsOpen", value);
			animator.speed = speed;
		}
		public void PickAnimation(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsPick", value);
			animator.speed = speed;
		}
	}
}
