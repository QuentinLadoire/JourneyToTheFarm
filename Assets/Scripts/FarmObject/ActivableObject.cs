using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public abstract class ActivableObject : SimpleObject
    {
        [Header("ActivableObject Parameter")]
        public float duration = 0.0f;
        public float animationDuration = 0.0f;
    
        protected bool isActived = false;

        protected float GetDesiredAnimationSpeed()
        {
            return duration == 0 ? 1.0f : animationDuration / duration;
        }
        public abstract void PlayAnim(AnimationController animationController);
        public abstract void StopAnim(AnimationController animationController);

        public virtual void Activate()
        {
            isActived = true;
        }
        public virtual void Desactivate()
        {
            isActived = false;
        }

        public abstract bool IsActivable();
        public abstract void ApplyEffect();
    }
}
