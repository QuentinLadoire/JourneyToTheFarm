using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public interface IEquipable
    {
        public void Equip(Transform rightHand, Transform leftHand);
    }
}
