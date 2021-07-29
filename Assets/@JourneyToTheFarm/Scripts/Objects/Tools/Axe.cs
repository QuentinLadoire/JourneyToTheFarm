using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Axe : CustomBehaviour, IHandable, IUsable
	{
		public float Duration => duration;
		public float AnimationDuration => animationDuration;
		public float AnimationMultiplier => animationMultiplier;

		[SerializeField] float duration = 0.0f;
		[SerializeField] float animationDuration = 0.0f;
		[SerializeField] float animationMultiplier = 0.0f;

		Tree tree = null;

		public void SetHanded(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public bool IsUsable()
		{
			if (Physics.Raycast(Player.Position + Vector3.up, Player.Forward, out RaycastHit hit, 1.0f))
			{
				tree = hit.collider.GetComponentInParent<Tree>();
				if (tree != null && tree.IsHarvestable())
					return true;
			}

			return false;
		}
		public void Use()
		{
			if (tree != null)
				tree.Harvest();
		}

		public void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterCutting(true, animationController.GetDesiredAnimationSpeed(duration, animationDuration, animationMultiplier));
		}
		public void StopAnim(AnimationController animationController)
		{
			animationController.CharacterCutting(false);
		}
	}
}
