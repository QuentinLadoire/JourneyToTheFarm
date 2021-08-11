using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public interface IUsable
    {
        public float Duration { get; }
        public ActionType ActionType { get; }
        public bool IsUsable { get; }

        public void Use();
    }
}
