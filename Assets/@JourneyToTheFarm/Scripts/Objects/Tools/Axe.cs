using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Axe : UsableBehaviour
	{
		Tree tree = null;
		PlayerInteractionText interactionText = null;

		protected override bool CheckIsUsable()
		{
			interactionText.SetActive(false);

			if (Physics.Raycast(OwnerPlayer.transform.position + Vector3.up, OwnerPlayer.transform.forward, out RaycastHit hit, 1.0f))
			{
				tree = hit.collider.GetComponentInParent<Tree>();
				if (tree != null && tree.IsHarvestable())
				{
					interactionText.SetText("Press E to Cut");
					interactionText.SetActive(true);

					return true;
				}
			}

			return false;
		}

		protected override void Awake()
		{
			base.Awake();

			interactionText = CanvasManager.GamePanel.PlayerPanel.PlayerInteractionText;
		}
		private void OnDestroy()
		{
			if (interactionText != null)
				interactionText.SetActive(false);
		}

		public override void Use()
		{
			if (tree != null)
				tree.Harvest(OwnerPlayer);
		}
	}
}
