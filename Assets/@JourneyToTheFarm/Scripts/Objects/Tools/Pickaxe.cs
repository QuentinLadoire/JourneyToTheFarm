using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Pickaxe : OwnableBehaviour, IEquipable, IUsable
	{
		[Header("Pickaxe")]
		public float duration = 0.0f;

		Rock rock = null;
		bool isUsable = false;
		PlayerInteractionText interactionText = null;

		public bool IsUsable => isUsable;
		public float Duration => duration;
		public ActionType ActionType => ActionType.Mine;

		void CheckIsUsable()
		{
			if (Physics.Raycast(OwnerPlayer.transform.position + new Vector3(0.0f, 0.2f, 0.0f), OwnerPlayer.transform.forward, out RaycastHit hit, 0.7f))
			{
				rock = hit.collider.GetComponentInParent<Rock>();
				if (rock != null && rock.IsHarvestable())
				{
					interactionText.SetText("Press E to Mine");
					interactionText.SetActive(true);

					isUsable = true;
				}
			}
			else
			{
				interactionText.SetActive(false);

				isUsable = false;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			interactionText = CanvasManager.GamePanel.PlayerPanel.PlayerInteractionText;
		}
		private void Update()
		{
			CheckIsUsable();
		}
		private void OnDestroy()
		{
			if (interactionText != null)
				interactionText.SetActive(false);
		}

		public void Equip(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public void Use()
		{
			if (rock != null)
				rock.Harvest(OwnerPlayer);
		}
	}
}
