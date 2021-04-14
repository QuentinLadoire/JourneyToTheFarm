using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmAxe : ActivableObject
	{
		TransportableObject transportableObject = null;
		Transform rightHandTransform = null;

		Tree tree = null;

		private void Awake()
		{
			transportableObject = GetComponent<TransportableObject>();
			transportableObject.onSetHands += OnSetHands;
		}
		private void Update()
		{
			if (rightHandTransform != null)
				transform.up = rightHandTransform.position - transform.position;
		}
		private void OnDestroy()
		{
			transportableObject.onSetHands -= OnSetHands;
		}

		void OnSetHands(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(leftHand, false);
			rightHandTransform = rightHand;
		}

		public override void ApplyEffect()
		{
			Debug.Log("TIMBER !!!!");
			var tmp = Player.AddItem(ItemType.Resource, "Log", 3);
			Debug.Log(tmp);
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
			Debug.Log("PlayAnim is not implemented");
		}
		public override void StopAnim(AnimationController animationController)
		{
			Debug.Log("StopAnim is not implemented");
		}
	}
}
