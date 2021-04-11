using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public abstract class SelectableObject : ActivableObject
    {
        public abstract void Select();
        public abstract void Deselect();
    }
}
