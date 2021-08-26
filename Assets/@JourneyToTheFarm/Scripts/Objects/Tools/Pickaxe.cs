using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Pickaxe : UsableBehaviour
	{
		Rock rock = null;
		PlayerInteractionText interactionText = null;

		protected override bool CheckIsUsable()
		{
			interactionText.SetActive(false);

			if (Physics.Raycast(OwnerPlayer.transform.position + new Vector3(0.0f, 0.2f, 0.0f), OwnerPlayer.transform.forward, out RaycastHit hit, 0.7f))
			{
				rock = hit.collider.GetComponentInParent<Rock>();
				if (rock != null && rock.IsHarvestable())
				{
					interactionText.SetText("Press E to Mine");
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
			if (rock != null)
				rock.Harvest(OwnerPlayer);
		}
	}
}
