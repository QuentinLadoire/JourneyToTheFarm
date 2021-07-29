using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public interface IInteractable
    {
        public float Duration { get; }
        public float AnimationDuration { get; }
        public float AnimationMultiplier { get; }

        public void Select();
        public void Deselect();

        public bool IsInteractable();
        public void Interact();

        public void PlayAnim(AnimationController animationController);
        public void StopAnim(AnimationController animationController);
    }
}
