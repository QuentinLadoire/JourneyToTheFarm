using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class TPCameraController : MonoBehaviour
	{
		[SerializeField] GameObject cameraObject = null;
		[SerializeField] GameObject followingObject = null;

		[SerializeField] float speed = 5.0f;
		[SerializeField] float angularSpeed = 1.0f;

		[SerializeField] float farOffset = -5.0f;

		[SerializeField] Vector3 followingOffset = Vector3.zero;

		void CameraRotation()
		{
			cameraObject.transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * angularSpeed);
			cameraObject.transform.RotateAround(transform.position, cameraObject.transform.right, -Input.GetAxis("Mouse Y") * angularSpeed);
		}
		void CameraFarOffset()
		{
			if (Physics.Raycast(transform.position, -cameraObject.transform.forward, out RaycastHit hit, -farOffset))
				cameraObject.transform.position = hit.point;
			else
				cameraObject.transform.localPosition = cameraObject.transform.forward * farOffset;
		}
		void CameraFollowing()
		{
			if (followingObject == null) return;

			transform.position = Vector3.MoveTowards(transform.position, followingObject.transform.position + followingOffset, speed * Time.deltaTime);
		}

		private void Awake()
		{
			GameManager.playerCamera = cameraObject.GetComponent<Camera>();
		}
		private void Update()
		{
			if (Player.HasControl)
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
	}
}
