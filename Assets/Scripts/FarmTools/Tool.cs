using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public abstract class Tool : MonoBehaviour
    {
        [Header("Tool Parameter")]
        public float duration = 0.0f;

        public abstract bool IsUsable();
        public abstract void Use();
        public abstract void Unuse();
        public abstract void ApplyEffect();
        public abstract void PlayAnim(AnimationController animationController);
        public abstract void StopAnim(AnimationController animationController);

        public void Destroy()
		{
            Destroy(gameObject);
		}
	}
}
