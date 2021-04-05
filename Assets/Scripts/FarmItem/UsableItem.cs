using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public abstract class UsableItem : MonoBehaviour
    {
        [Header("UsableItem Parameter")]
        public float duration = 0.0f;
        public float multiplier = 1.0f;

        protected bool isUsed = false;

        public virtual void Use()
        {
            isUsed = true;
        }
        public virtual void Unuse()
        {
            isUsed = false;
        }
        public virtual void Destroy()
		{
            Destroy(gameObject);
		}

        public abstract void Init(Transform rightHand, Transform leftHand);
        public abstract bool IsUsable();
        public abstract void ApplyEffect();
        public abstract void PlayAnim(AnimationController animationController);
        public abstract void StopAnim(AnimationController animationController);
	}
}
