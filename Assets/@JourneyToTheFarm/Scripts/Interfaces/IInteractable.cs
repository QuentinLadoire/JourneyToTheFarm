using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public interface IInteractable
    {
        public bool IsInteractable { get; }
        public float ActionDuration { get; }
        public ActionType ActionType { get; }

        public void Select();
        public void Deselect();

        public void StartToInteract();
        public void Interact(Player player);
    }
}
