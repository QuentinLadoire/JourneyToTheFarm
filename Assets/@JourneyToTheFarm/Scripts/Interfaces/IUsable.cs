using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public interface IUsable
    {
        public float Duration { get; }
        public float AnimationDuration { get; }
        public float AnimationMultiplier { get; }

        public bool IsUsable();
        public void Use();

        public void PlayAnim(AnimationController animationController);
        public void StopAnim(AnimationController animationController);
    }
}
