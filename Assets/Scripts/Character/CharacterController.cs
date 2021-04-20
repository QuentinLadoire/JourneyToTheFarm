using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class CharacterController : MonoBehaviour
	{
		public Action onMoveEnter = () => { /*Debug.Log("OnHasMove");*/ };
		public Action onMoveExit = () => { /*Debug.Log("OnHasIdle");*/ };
		public Action<Vector3> onMove = (Vector3) => { /*Debug.Log("OnMovement");*/ };
		public Action<Vector3> onHasMoved = (Vector3) => { /*Debug.Log("HasMove");*/ };

		public Vector3 RoundPosition { get; private set; } = Vector3.zero;
		public bool IsIdle { get; private set; } = true;

		[SerializeField] GameObject followingCamera = null;
		[SerializeField] float speed = 1.0f;

		AnimationController animationController = null;

		Vector3 previousDirection = Vector3.zero;
		Vector3 previousPosition = Vector3.zero;

		void CharacterMovementEvent(Vector3 direction)
		{
			if (direction != Vector3.zero && previousDirection == Vector3.zero)
				onMoveEnter.Invoke();
			if (direction == Vector3.zero && previousDirection != Vector3.zero)
				onMoveExit.Invoke();

			if (direction != Vector3.zero)
				onMove.Invoke(direction);

			previousDirection = direction;

			Vector3 previousRoundPosition = new Vector3(Mathf.RoundToInt(previousPosition.x), Mathf.RoundToInt(previousPosition.y), Mathf.RoundToInt(previousPosition.z));
			RoundPosition = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
			if (previousRoundPosition != RoundPosition)
				onHasMoved.Invoke(RoundPosition);

			previousPosition = transform.position;
		}
		void CharacterMovement()
		{
			Vector3 direction = (Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.forward).normalized;
			transform.position += direction * speed * Time.deltaTime;

			CharacterMovementEvent(direction);
		}
		void CharacterRotation()
		{
			if (followingCamera == null) { Debug.LogError("Following Camera is Null"); return; }
			if (IsIdle == true) return;

			var cameraRotation = Quaternion.LookRotation(Vector3.Scale(followingCamera.transform.forward, new Vector3(1.0f, 0.0f, 1.0f)));
			transform.rotation = cameraRotation;
		}

		void OnMoveEnter()
		{
			IsIdle = false;
		}
		void OnMoveExit()
		{
			IsIdle = true;

			animationController.CharacterMouvementAnim(Vector3.zero);
		}
		void OnMove(Vector3 direction)
		{
			animationController.CharacterMouvementAnim(direction);
		}

		private void Awake()
		{
			animationController = GetComponent<AnimationController>();

			onMoveEnter += OnMoveEnter;
			onMoveExit += OnMoveExit;
			onMove += OnMove;
		}
		private void FixedUpdate()
		{
			if (Player.HasControl)
			{
				CharacterMovement();

				CharacterRotation();
			}
		}
		private void OnDestroy()
		{
			onMoveEnter -= OnMoveEnter;
			onMoveExit -= OnMoveExit;
			onMove -= OnMove;
		}
	}
}
