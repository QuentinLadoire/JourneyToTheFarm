using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class EquipableBehaviour : OwnableBehaviour, IEquipable
	{
		public virtual void Equip(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}
	}
}
