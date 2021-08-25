using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Scythe : UsableBehaviour
	{
		[Header("Scythe Settings")]
		[SerializeField] LayerMask overlapLayer = -1;

		PlayerInteractionText interactionText = null;
		readonly List<Grass> grassList = new List<Grass>();

		void CheckMowGrass()
		{
			grassList.Clear();

			var colliders = Physics.OverlapSphere(OwnerPlayer.transform.position, 1.5f, overlapLayer);
			foreach (var collider in colliders)
			{
				var colliderDirection = collider.transform.position - OwnerPlayer.transform.position;
				var dot = Vector3.Dot(colliderDirection, OwnerPlayer.transform.forward);
				if (dot > 0.0f)
				{
					var grass = collider.GetComponentInParent<Grass>();
					if (grass != null)
						grassList.Add(grass);
				}
			}
		}
		void CheckIsUsable()
		{
			CheckMowGrass();

			if (grassList.Count > 0)
			{
				interactionText.SetText("Press E to Mow");
				interactionText.SetActive(true);

				isUsable = true;
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

		public override void Use()
		{
			foreach (var grass in grassList)
				grass.Harvest(OwnerPlayer);

			grassList.Clear();
		}
	}
}
