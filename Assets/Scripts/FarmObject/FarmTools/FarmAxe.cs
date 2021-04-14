using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmAxe : ActivableObject
	{
		TransportableObject transportableObject = null;

		Tree tree = null;

		private void Awake()
		{
			transportableObject = GetComponent<TransportableObject>();
			transportableObject.onSetHands += OnSetHands;
		}
		private void OnDestroy()
		{
			transportableObject.onSetHands -= OnSetHands;
		}

		void OnSetHands(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public override void ApplyEffect()
		{
			Player.AddItem(ItemType.Resource, "Log", 3);
		}
		public override bool IsActivable()
		{
			RaycastHit hit;
			if (Physics.Raycast(Player.Position + Vector3.up, Player.Forward, out hit))
			{
				tree = hit.collider.GetComponentInParent<Tree>();
				if (tree != null)
					return true;
			}

			return false;
		}
		public override void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterCutting(true, GetDesiredAnimationSpeed());
		}
		public override void StopAnim(AnimationController animationController)
		{
			animationController.CharacterCutting(false);
		}
	}
}
