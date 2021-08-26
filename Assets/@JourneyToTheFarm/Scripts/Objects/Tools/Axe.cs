using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Axe : UsableBehaviour
	{
		Tree tree = null;

		private void UpdateFeedback()
		{
			if (IsUsable)
			{
				InteractionText.SetText("Press E to Cut");
				InteractionText.SetActive(true);
			}
			else
			{
				InteractionText.SetActive(false);
			}
		}

		protected override bool CheckIsUsable()
		{
			if (Physics.Raycast(OwnerPlayer.transform.position + Vector3.up, OwnerPlayer.transform.forward, out RaycastHit hit, 1.0f))
			{
				tree = hit.collider.GetComponentInParent<Tree>();
				if (tree != null && tree.IsHarvestable())
					return true;
			}

			return false;
		}

		protected override void Update()
		{
			base.Update();

			UpdateFeedback();
		}

		public override void Use()
		{
			if (tree != null)
				tree.Harvest(OwnerPlayer);
		}
	}
}
