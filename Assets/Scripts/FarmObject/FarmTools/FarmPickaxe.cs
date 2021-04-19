using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmPickaxe : ActivableObject
	{
		TransportableObject transportableObject = null;

		Rock rock = null;

		private void Awake()
		{
			transportableObject = GetComponent<TransportableObject>();
			transportableObject.onSetHands += OnSetHand;
		}
		private void OnDestroy()
		{
			transportableObject.onSetHands -= OnSetHand;
		}

		void OnSetHand(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public override bool IsActivable()
		{
			RaycastHit hit;
			if (Physics.Raycast(Player.Position + new Vector3(0.0f, 0.2f, 0.0f), Player.Forward, out hit, 0.7f))
			{
				rock = hit.collider.GetComponentInParent<Rock>();
				if (rock != null && rock.IsHarvestable())
					return true;
			}

			return false;
		}
		public override void ApplyEffect()
		{
			if (rock != null)
				rock.Harvest();
		}
		public override void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterMining(true, GetDesiredAnimationSpeed());
		}
		public override void StopAnim(AnimationController animationController)
		{
			animationController.CharacterMining(false);
		}
	}
}
