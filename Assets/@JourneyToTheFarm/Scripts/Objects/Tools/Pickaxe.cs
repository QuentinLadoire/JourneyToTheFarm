using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;

namespace JTTF.Gameplay
{
	public class Pickaxe : UsableBehaviour
	{
		private Rock rock = null;

		private void UpdateFeedback()
		{
			if (IsUsable)
			{
				InteractionText.SetText("Press E to Mine");
				InteractionText.SetActive(true);
			}
			else
			{
				InteractionText.SetActive(false);
			}
		}
		protected override bool CheckIsUsable()
		{
			if (Physics.Raycast(OwnerPlayer.transform.position + new Vector3(0.0f, 0.2f, 0.0f), OwnerPlayer.transform.forward, out RaycastHit hit, 0.7f))
			{
				rock = hit.collider.GetComponentInParent<Rock>();
				if (rock != null && rock.IsHarvestable())
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
			if (rock != null)
				rock.Harvest();
		}
	}
}
