using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public abstract class ActivableObject : MonoBehaviour
    {
        [Header("ActivableObject Parameter")]
        public float duration = 0.0f;

        protected bool isActived = false;

        public virtual void Activate()
        {
            isActived = true;
        }
        public virtual void Desactivate()
        {
            isActived = false;
        }
        public virtual void Destroy()
        {
            Destroy(gameObject);
        }

        public abstract void Select();
        public abstract void Deselect();
        public abstract bool IsActivable();
        public abstract void ApplyEffect();
        public abstract void PlayAnim(AnimationController animationController);
        public abstract void StopAnim(AnimationController animationController);
    }
}
