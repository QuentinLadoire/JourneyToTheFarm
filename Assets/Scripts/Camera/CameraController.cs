using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] GameObject cameraObject = null;
		[SerializeField] GameObject followingObject = null;

		[SerializeField] float speed = 1.0f;
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
			RaycastHit hit;
			if (Physics.Raycast(transform.position, -cameraObject.transform.forward, out hit, -farOffset, LayerMask.GetMask("World")))
				cameraObject.transform.position = hit.point;
			else
				cameraObject.transform.localPosition = cameraObject.transform.forward * farOffset;
		}
		void CameraFollowing()
		{
			if (followingObject == null) return;

			transform.position = Vector3.MoveTowards(transform.position, followingObject.transform.position + followingOffset, speed * Time.deltaTime);
		}

		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		private void Update()
		{
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
