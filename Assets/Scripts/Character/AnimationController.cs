using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class AnimationController : MonoBehaviour
    {
        Animator animator = null;
		CharacterController characterController = null;

		private void Awake()
		{
			animator = GetComponentInChildren<Animator>();
			characterController = GetComponent<CharacterController>();
		}
		private void Start()
		{
			characterController.onMove += CharacterMouvementAnim;
		}

		public void CharacterMouvementAnim(Vector3 direction)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			var localDirection = transform.worldToLocalMatrix * direction;
			animator.SetFloat("DirectionX", localDirection.x);
			animator.SetFloat("DirectionY", localDirection.z);
		}
		public void CharacterDiggingAnim(bool value)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsDig", value);
		}
		public void CharacterPlantAPlant(bool value)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsPlant", value);
		}
	}
}
