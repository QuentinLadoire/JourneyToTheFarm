using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class AnimationController : MonoBehaviour
    {
        Animator animator = null;

		private void Awake()
		{
			animator = GetComponentInChildren<Animator>();
		}

		public void CharacterMouvementAnim(Vector3 direction, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			var localDirection = transform.worldToLocalMatrix * direction;
			animator.SetFloat("DirectionX", localDirection.x);
			animator.SetFloat("DirectionY", localDirection.z);
			animator.speed = speed;
		}
		public void CharacterDiggingAnim(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsDig", value);
			animator.speed = speed;
		}
		public void CharacterPlantAPlant(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsPlant", value);
			animator.speed = speed;
		}
		public void CharacterPickUp(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }

			animator.SetBool("IsPick", value);
			animator.speed = speed;
		}
		public void CharacterCutting(bool value, float speed = 1.0f)
		{
			if (animator == null) { Debug.LogError("Animator is Null"); return; }
			animator.SetBool("IsCut", value);
			animator.speed = speed;
		}
	}
}
