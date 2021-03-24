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

		[SerializeField] GameObject followingCamera = null;
		[SerializeField] float speed = 1.0f;

		AnimationController animationController = null;

		void CharacterMovement()
		{
			Vector3 direction = (Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.forward).normalized;
			transform.position += direction * speed * Time.deltaTime;

			animationController.CharacterMouvementAnim(direction);
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

			animationController = GetComponent<AnimationController>();
		}
		private void FixedUpdate()
		{
			CharacterMovement();

			CharacterRotation();
		}
	}
}
