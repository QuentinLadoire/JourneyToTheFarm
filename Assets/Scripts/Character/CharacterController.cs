using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class CharacterController : MonoBehaviour
	{
		[SerializeField] GameObject followingCamera = null;
		[SerializeField] Animator animator = null;

		[SerializeField] float speed = 1.0f;

		void CharacterMouvementAnim(Vector3 direction)
		{
			if (animator == null) return;

			var localDirection = transform.worldToLocalMatrix * direction;
			animator.SetFloat("DirectionX", localDirection.x);
			animator.SetFloat("DirectionY", localDirection.z);
		}
		void CharacterMovement()
		{
			Vector3 direction = (Input.GetAxisRaw("Horizontal") * transform.right + Input.GetAxisRaw("Vertical") * transform.forward).normalized;
			transform.position += direction * speed * Time.deltaTime;

			CharacterMouvementAnim(direction);
		}
		void CharacterRotation()
		{
			if (followingCamera == null) return;

			var cameraRotation = Quaternion.LookRotation(Vector3.Scale(followingCamera.transform.forward, new Vector3(1.0f, 0.0f, 1.0f)));
			transform.rotation = cameraRotation;
		}

		private void Update()
		{
			CharacterMovement();

			CharacterRotation();
		}
	}
}
