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
	}
}
