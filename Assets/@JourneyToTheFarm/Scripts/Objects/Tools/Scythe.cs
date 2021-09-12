using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;

namespace JTTF.Gameplay
{
	public class Scythe : UsableBehaviour
	{
		[Header("Scythe Settings")]
		[SerializeField] private LayerMask overlapLayer = -1;

		readonly private List<Grass> grassList = new List<Grass>();

		private void CheckOverlapGrass()
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
		private void UpdateFeedback()
		{
			if (IsUsable)
			{
				InteractionText.SetText("Press E to Mow");
				InteractionText.SetActive(true);
			}
			else
			{
				InteractionText.SetActive(false);
			}
		}

		protected override bool CheckIsUsable()
		{
			CheckOverlapGrass();

			return (grassList.Count > 0);
		}

		protected override void Update()
		{
			base.Update();

			UpdateFeedback();
		}

		public override void Use()
		{
			foreach (var grass in grassList)
				grass.Harvest(OwnerPlayer);

			grassList.Clear();
		}
	}
}
