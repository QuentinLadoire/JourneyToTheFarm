using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public interface IUsable
    {
        public bool IsUsable { get; }
        public float ActionDuration { get; }
        public ActionType ActionType { get; }

        public void Use();
    }
}
