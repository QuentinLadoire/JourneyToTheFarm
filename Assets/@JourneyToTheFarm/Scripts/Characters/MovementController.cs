using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;
using JTTF.Management;

#pragma warning disable IDE0044

namespace JTTF.Character
{
	public class MovementController : CustomNetworkBehaviour
	{
		[SerializeField] private float moveSpeed = 5.0f;
		[SerializeField] private float jumpForce = 5.0f;
		[SerializeField] private LayerMask layer = -1;
		[SerializeField] private TPCameraController cameraController = null;

		private bool hasJump = false;
		private bool wantJump = false;
		private bool hasControl = true;
		private Player ownerPlayer = null;
		private new Rigidbody rigidbody = null;
		private GameObject followingCamera = null;

		private Vector3 direction = Vector3.zero;
		private Vector3 previousDirection = Vector3.zero;
		private Vector3 previousPosition = Vector3.zero;

		public bool HasControl => hasControl;
		public Vector3 Direction => direction;
		public Player OwnerPlayer => ownerPlayer;
		public bool IsIdle => direction == Vector3.zero;
		public Vector3 RoundForward => transform.forward.RoundToInt();
		public Vector3 RoundPosition => transform.position.RoundToInt();

		public Action onHasJump = () => { /*Debug.Log("HasJump");*/ };
		public Action onHasMoved = () => { /*Debug.Log("HasMove");*/ };
		public Action onMoveExit = () => { /*Debug.Log("OnHasIdle");*/ };
		public Action onMoveEnter = () => { /*Debug.Log("OnHasMove");*/ };

		private void ProcessInput()
		{
			var x = Input.GetAxisRaw("Horizontal") * transform.right;
			var z = Input.GetAxisRaw("Vertical") * transform.forward;

			wantJump = Input.GetButtonDown("Jump") && !hasJump;

			previousDirection = direction;
			direction = (x + z).normalized;
		}
		private void ProcessMovement()
		{
			previousPosition = transform.position;
			rigidbody.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
		}
		private void ProcessJump()
		{
			hasJump = false;
			if (wantJump && Physics.CheckSphere(transform.position + Vector3.up * 0.5f, 0.55f, layer))
			{
				rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
				hasJump = true;
			}
		}
		private void ProcessRotation()
		{
			if (followingCamera == null) { Debug.LogWarning("Following Camera is Null"); return; }
			if (IsIdle == true) return;

			var cameraRotation = Quaternion.LookRotation(Vector3.Scale(followingCamera.transform.forward, new Vector3(1.0f, 0.0f, 1.0f)));
			transform.rotation = cameraRotation;
		}

		private void ProcessEvents()
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

		protected override void Awake()
		{
			base.Awake();

			ownerPlayer = GetComponent<Player>();
			rigidbody = GetComponent<Rigidbody>();
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

			if (cameraController == null)
				cameraController = Instantiate(GameManager.PrefabDataBase.CameraControllerPrefab).GetComponent<TPCameraController>();

			SetCameraController(cameraController);
		}
		protected override void Update()
		{
			base.Update();

			if (HasControl)
				ProcessInput();

			ProcessRotation();
			ProcessJump();
		}
		protected virtual void FixedUpdate()
		{
			ProcessMovement();
		}
		protected virtual void LateUpdate()
		{
			ProcessEvents();
		}

		public void SetCameraController(TPCameraController cameraController)
		{
			this.cameraController = cameraController;
			this.cameraController.SetFollowingObject(gameObject);
			followingCamera = cameraController.CameraObject;
		}

		private int deactiveCount = 0;
		public void ActiveControl()
		{
			if (deactiveCount > 0)
				deactiveCount--;

			if (deactiveCount == 0)
				hasControl = true;
		}
		public void DeactiveControl()
		{
			deactiveCount++;
			hasControl = false;
		}
	}
}
