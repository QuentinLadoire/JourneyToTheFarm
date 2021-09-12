using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;

namespace JTTF.Interface
{
    public interface IUsable
    {
        public bool IsUsable { get; }
        public float ActionDuration { get; }
        public ActionType ActionType { get; }

        public void Use();
    }
}
