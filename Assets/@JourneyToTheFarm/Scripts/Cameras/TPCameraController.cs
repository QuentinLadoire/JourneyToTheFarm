using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;
using JTTF.Management;

#pragma warning disable IDE0044

namespace JTTF
{
	public class TPCameraController : CustomBehaviour
	{
		[SerializeField] private bool hasControl = true;
		[SerializeField] private float speed = 5.0f;
		[SerializeField] private float farOffset = -5.0f;
		[SerializeField] private float angularSpeed = 1.0f;
		[SerializeField] private new Camera camera = null;
		[SerializeField] private Vector3 followingOffset = Vector3.zero;

		private GameObject followingObject = null;

		public Camera Camera => camera;
		public bool HasControl => hasControl;
		public GameObject CameraObject => camera.gameObject;

		private void CameraRotation()
		{
			CameraObject.transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * angularSpeed);
			CameraObject.transform.RotateAround(transform.position, CameraObject.transform.right, -Input.GetAxis("Mouse Y") * angularSpeed);
		}
		private void CameraFarOffset()
		{
			if (Physics.Raycast(transform.position, -CameraObject.transform.forward, out RaycastHit hit, -farOffset))
				CameraObject.transform.position = hit.point;
			else
				CameraObject.transform.localPosition = CameraObject.transform.forward * farOffset;
		}
		private void CameraFollowing()
		{
			if (followingObject == null) return;

			transform.position = Vector3.MoveTowards(transform.position, followingObject.transform.position + followingOffset, speed * Time.deltaTime);
		}

		protected override void Awake()
		{
			base.Awake();

			GameManager.cameraController = this;
		}
		protected override void Update()
		{
			base.Update();

			if (HasControl)
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
				transform.position = followingObject.transform.position + followingOffset;
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
