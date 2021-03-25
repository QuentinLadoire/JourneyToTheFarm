using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class CharacterController : MonoBehaviour
	{
		static CharacterController instance = null;

		public static Vector3 Position { get => instance.transform.position; }
		public static Vector3 Forward { get => instance.transform.forward; }

		public Action onMoveEnter = () => { /*Debug.Log("OnHasMove");*/ };
		public Action onMoveExit = () => { /*Debug.Log("OnHasIdle");*/ };
		public Action<Vector3> onMove = (Vector3) => { /*Debug.Log("OnMovement");*/ };

		[SerializeField] GameObject followingCamera = null;
		[SerializeField] float speed = 1.0f;

		Vector3 previousDirection = Vector3.zero;

		void CharacterMovementEvent(Vector3 direction)
		{
			if (direction != Vector3.zero && previousDirection == Vector3.zero)
				onMoveEnter.Invoke();
			if (direction == Vector3.zero && previousDirection != Vector3.zero)
				onMoveExit.Invoke();

			previousDirection = direction;

			onMove.Invoke(direction);
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

			var cameraRotation = Quaternion.LookRotation(Vector3.Scale(followingCamera.transform.forward, new Vector3(1.0f, 0.0f, 1.0f)));
			transform.rotation = cameraRotation;
		}

		private void Awake()
		{
			instance = this;
		}
		private void FixedUpdate()
		{
			CharacterMovement();

			CharacterRotation();
		}
	}
}
