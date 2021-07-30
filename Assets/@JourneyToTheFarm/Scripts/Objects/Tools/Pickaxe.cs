using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Pickaxe : CustomBehaviour, IEquipable, IUsable
	{
		public float Duration => duration;
		public ActionType ActionType => ActionType.Mine;

		[Header("Pickaxe")]
		public float duration = 0.0f;

		Rock rock = null;

		public void Equip(Transform rightHand, Transform leftHand)
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
	}
}
