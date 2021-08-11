using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Axe : CustomBehaviour, IEquipable, IUsable, IOwnable
	{
		public Player OwnerPlayer { get; private set; }

		public float Duration => duration;
		public ActionType ActionType => ActionType.Cut;

		[Header("Axe Parameters")]
		public float duration = 0.0f;

		Tree tree = null;

		public void SetOwner(Player owner)
		{
			OwnerPlayer = owner;
		}

		public void Equip(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public bool IsUsable()
		{
			if (Physics.Raycast(OwnerPlayer.transform.position + Vector3.up, OwnerPlayer.transform.forward, out RaycastHit hit, 1.0f))
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
				tree.Harvest(OwnerPlayer);
		}

		private void Update()
		{
			if (tree != null)
			{

			}
		}
	}
}
