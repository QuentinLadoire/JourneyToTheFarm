using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class CharacterController : MonoBehaviour
	{
		public Player OwnerPlayer { get; private set; }

		public Action onMoveEnter = () => { /*Debug.Log("OnHasMove");*/ };
		public Action onMoveExit = () => { /*Debug.Log("OnHasIdle");*/ };
		public Action onHasMoved = () => { /*Debug.Log("HasMove");*/ };
		public Action onHasJump = () => { /*Debug.Log("HasJump");*/ };

		public bool HasControl { get; private set; } = true;
		public bool IsIdle => direction == Vector3.zero;
		public Vector3 RoundPosition => transform.position.RoundToInt();
		public Vector3 RoundForward => transform.forward.RoundToInt();
		public Vector3 Direction => direction;

		[SerializeField] TPCameraController cameraController = null;
		[SerializeField] float moveSpeed = 5.0f;
		[SerializeField] float jumpForce = 5.0f;
		[SerializeField] LayerMask layer = -1;

		GameObject followingCamera = null;
		new Rigidbody rigidbody = null;
		bool wantJump = false;
		bool hasJump = false;

		Vector3 direction = Vector3.zero;
		Vector3 previousDirection = Vector3.zero;
		Vector3 previousPosition = Vector3.zero;

		public void ActiveControl()
		{
			HasControl = true;
		}
		public void DesactiveControl()
		{
			HasControl = false;
		}

		void ProcessInput()
		{
			var x = Input.GetAxisRaw("Horizontal") * transform.right;
			var z = Input.GetAxisRaw("Vertical") * transform.forward;

			wantJump = Input.GetButtonDown("Jump") && !hasJump;

			previousDirection = direction;
			direction = (x + z).normalized;
		}
		void ProcessMovement()
		{
			previousPosition = transform.position;
			rigidbody.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
		}
		void ProcessJump()
		{
			hasJump = false;
			if (wantJump && Physics.CheckSphere(transform.position + Vector3.up * 0.5f, 0.55f, layer))
			{
				rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
				hasJump = true;
			}
		}
		void ProcessRotation()
		{
			if (followingCamera == null) { Debug.LogError("Following Camera is Null"); return; }
			if (IsIdle == true) return;

			var cameraRotation = Quaternion.LookRotation(Vector3.Scale(followingCamera.transform.forward, new Vector3(1.0f, 0.0f, 1.0f)));
			transform.rotation = cameraRotation;
		}

		void ProcessEvents()
		{
			if (direction != Vector3.zero && previousDirection == Vector3.zero)
				onMoveEnter.Invoke();
			if (direction == Vector3.zero && previousDirection != Vector3.zero)
				onMoveExit.Invoke();

			Vector3 previousRoundPosition = previousPosition.RoundToInt();
			if (previousRoundPosition != RoundPosition)
				onHasMoved.Invoke();

			if (hasJump)
				onHasJump.Invoke();
		}

		private void Awake()
		{
			OwnerPlayer = GetComponent<Player>();
			rigidbody = GetComponent<Rigidbody>();
		}
		private void Start()
		{
			cameraController.SetOwner(OwnerPlayer);
			followingCamera = cameraController.CameraObject;
		}
		private void Update()
		{
			if (HasControl)
				ProcessInput();

			ProcessRotation();
			ProcessJump();
		}
		private void FixedUpdate()
		{
			ProcessMovement();
		}
		private void LateUpdate()
		{
			ProcessEvents();
		}
	}
}
