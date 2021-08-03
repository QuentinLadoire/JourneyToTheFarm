using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public interface IInteractable
    {
        public float Duration { get; }
        public ActionType ActionType { get; }

        public void Select();
        public void Deselect();

        public bool IsInteractable();
        public void StartToInteract();
        public void Interact(Player player);
    }
}
