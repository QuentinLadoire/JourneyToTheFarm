using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF.Interface
{
    public interface IEquipable
    {
        public void Equip(Transform rightHand, Transform leftHand);
    }
}
