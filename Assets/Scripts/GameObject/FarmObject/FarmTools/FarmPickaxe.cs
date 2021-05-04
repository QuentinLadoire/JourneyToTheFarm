using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmPickaxe : SimpleObject, IHandable, IUsable
	{
		public float Duration => duration;
		public float AnimationDuration => animationDuration;
		public float AnimationMultiplier => animationMultiplier;

		[SerializeField] float duration = 0.0f;
		[SerializeField] float animationDuration = 0.0f;
		[SerializeField] float animationMultiplier = 1.0f;

		Rock rock = null;

		public void SetHanded(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public bool IsUsable()
		{
			if (Physics.Raycast(Player.Position + new Vector3(0.0f, 0.2f, 0.0f), Player.Forward, out RaycastHit hit, 0.7f))
			{
				rock = hit.collider.GetComponentInParent<Rock>();
				if (rock != null && rock.IsHarvestable())
					return true;
			}

			return false;
		}
		public void Use()
		{
			if (rock != null)
				rock.Harvest();
		}
		public void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterMining(true, animationController.GetDesiredAnimationSpeed(duration, animationDuration, animationMultiplier));
		}
		public void StopAnim(AnimationController animationController)
		{
			animationController.CharacterMining(false);
		}
	}
}
