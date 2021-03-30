using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmAxe : UsableItem
	{
		public override void ApplyEffect()
		{
			Debug.Log("ApplyEffect is not implemented");
		}
		public override void Init(Transform rightHand, Transform leftHand)
		{
			Debug.Log("Init is not implemented");
		}
		public override bool IsUsable()
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
