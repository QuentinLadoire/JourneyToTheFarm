using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable IDE0044
#pragma warning disable IDE0051

namespace JTTF
{
	public class TPCameraController : CustomBehaviour
	{
		[SerializeField] private float speed = 5.0f;
		[SerializeField] private float farOffset = -5.0f;
		[SerializeField] private float angularSpeed = 1.0f;
		[SerializeField] private GameObject cameraObject = null;
		[SerializeField] private Vector3 followingOffset = Vector3.zero;

		private GameObject followingObject = null;
		private MovementController characterController = null;

		public GameObject CameraObject => cameraObject;

		private void CameraRotation()
		{
			cameraObject.transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * angularSpeed);
			cameraObject.transform.RotateAround(transform.position, cameraObject.transform.right, -Input.GetAxis("Mouse Y") * angularSpeed);
		}
		private void CameraFarOffset()
		{
			if (Physics.Raycast(transform.position, -cameraObject.transform.forward, out RaycastHit hit, -farOffset))
				cameraObject.transform.position = hit.point;
			else
				cameraObject.transform.localPosition = cameraObject.transform.forward * farOffset;
		}
		private void CameraFollowing()
		{
			if (followingObject == null) return;

			transform.position = Vector3.MoveTowards(transform.position, followingObject.transform.position + followingOffset, speed * Time.deltaTime);
		}

		protected override void Awake()
		{
			base.Awake();

			GameManager.playerCamera = cameraObject.GetComponent<Camera>();
		}
		protected override void Update()
		{
			base.Update();

			if (characterController.HasControl)
				CameraRotation();

			CameraFarOffset();
		}
		private void FixedUpdate()
		{
			CameraFollowing();
		}
		private void OnValidate()
		{
			if (followingObject != null)
				transform.position = followingObject.transform.position + followingOffset;
		}

		public void SetFollowingObject(GameObject obj)
		{
			followingObject = obj;

			if (followingObject != null)
			{
				characterController = followingObject.GetComponent<MovementController>();
				transform.position = followingObject.transform.position + followingOffset;
			}
		}
	}
}
