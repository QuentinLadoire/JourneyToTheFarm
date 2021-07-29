using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public interface IHandable
    {
        public void SetHanded(Transform rightHand, Transform leftHand);
    }
}
