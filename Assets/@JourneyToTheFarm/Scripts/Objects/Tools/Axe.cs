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
		public bool IsUsable => isUsable;

		[Header("Axe Parameters")]
		public float duration = 0.0f;

		Tree tree = null;
		bool isUsable = false;
		PlayerInteractionText interactionText = null;

		void CheckIsUsable()
		{
			if (Physics.Raycast(OwnerPlayer.transform.position + Vector3.up, OwnerPlayer.transform.forward, out RaycastHit hit, 1.0f))
			{
				tree = hit.collider.GetComponentInParent<Tree>();
				if (tree != null && tree.IsHarvestable())
				{
					interactionText.SetText("Press E to Cut");
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

		public void SetOwner(Player owner)
		{
			OwnerPlayer = owner;
		}

		public void Equip(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public void Use()
		{
			if (tree != null)
				tree.Harvest(OwnerPlayer);
		}
	}
}
