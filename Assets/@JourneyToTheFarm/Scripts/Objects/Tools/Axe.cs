using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Axe : CustomBehaviour, IEquipable, IUsable
	{
		public float Duration => duration;
		public ActionType ActionType => ActionType.Cut;

		[Header("Axe Parameters")]
		public float duration = 0.0f;

		Tree tree = null;

		public void Equip(Transform rightHand, Transform leftHand)
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
	}
}
