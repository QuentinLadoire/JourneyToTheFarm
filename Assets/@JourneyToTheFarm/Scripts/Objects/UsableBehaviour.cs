using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class UsableBehaviour : EquipableBehaviour, IUsable
	{
		[Header("UsableBehaviour Settings")]
		[SerializeField] float actionDuration = 0.0f;
		[SerializeField] ActionType actionType = ActionType.None;

		protected bool isUsable = false;

		public bool IsUsable => isUsable;
		public float ActionDuration => actionDuration;
		public ActionType ActionType => actionType;

		public virtual void Use()
		{
			Debug.LogWarning("The Mehtod Use of " + this.GetType().ToString() + ", is not Implemented.");
		}
	}
}
