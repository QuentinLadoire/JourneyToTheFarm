using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class CharacterController : MonoBehaviour
	{
		[SerializeField] readonly private float moveSpeed = 5.0f;
		[SerializeField] readonly private float jumpForce = 5.0f;
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

		protected virtual void Awake()
		{
			ownerPlayer = GetComponent<Player>();
			rigidbody = GetComponent<Rigidbody>();
		}
		protected virtual void Start()
		{
			if (cameraController == null)
				cameraController = Instantiate(GameManager.PrefabDataBase.CameraControllerPrefab).GetComponent<TPCameraController>();

			SetCameraController(cameraController);
		}
		protected virtual void Update()
		{
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

		public void ActiveControl()
		{
			hasControl = true;
		}
		public void DesactiveControl()
		{
			hasControl = false;
		}
	}
}
