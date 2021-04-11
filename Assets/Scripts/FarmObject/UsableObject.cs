using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    namespace Deprecated
    {
        public abstract class UsableObject : SimpleObject
        {
            [Header("UsableItem Parameter")]
            public float duration = 0.0f;
            public float animationDuration = 0.0f;

            protected bool isUsed = false;

            protected float GetDesiredAnimationSpeed()
            {
                return duration == 0 ? 1.0f : animationDuration / duration;
            }
            public abstract void PlayAnim(AnimationController animationController);
            public abstract void StopAnim(AnimationController animationController);

            public virtual void Use()
            {
                isUsed = true;
            }
            public virtual void Unuse()
            {
                isUsed = false;
            }

            public abstract bool IsUsable();
            public abstract void ApplyEffect();

            public abstract void Init(Transform rightHand, Transform leftHand);
        }
    }
}
