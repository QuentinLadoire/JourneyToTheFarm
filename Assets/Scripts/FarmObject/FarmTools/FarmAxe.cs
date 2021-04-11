using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmAxe : ActivableObject
	{
		public override void ApplyEffect()
		{
			Debug.Log("ApplyEffect is not implemented");
		}
		public override bool IsActivable()
		{
			Debug.Log("IsUsable is not implemented");

			return false;
		}
		public override void PlayAnim(AnimationController animationController)
		{
			Debug.Log("PlayAnim is not implemented");
		}
		public override void StopAnim(AnimationController animationController)
		{
			Debug.Log("StopAnim is not implemented");
		}
	}
}
