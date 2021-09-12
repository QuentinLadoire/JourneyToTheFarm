using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Interface;

namespace JTTF.Behaviour
{
	public class EquipableBehaviour : OwnableBehaviour, IEquipable
	{
		public virtual void Equip(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}
	}
}
