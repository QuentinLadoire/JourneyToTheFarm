using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Scythe : CustomBehaviour, IEquipable, IUsable
	{
		public float Duration => duration;
		public ActionType ActionType => ActionType.Mow;

		[Header("Scythe Parameters")]
		public float duration = 0.0f;
		public LayerMask overlapLayer = -1;

		List<Grass> grassList = new List<Grass>();

		public void Equip(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public bool IsUsable()
		{
			CheckMowGrass();
			Debug.Log(grassList.Count);
			return grassList.Count > 0;
		}
		public void Use()
		{
			foreach (var grass in grassList)
				grass.Harvest();

			grassList.Clear();
		}

		void CheckMowGrass()
		{
			grassList.Clear();

			var colliders = Physics.OverlapSphere(Player.Position, 1.5f, overlapLayer);
			foreach (var collider in colliders)
			{
				var colliderDirection = collider.transform.position - Player.Position;
				var dot = Vector3.Dot(colliderDirection, Player.Forward);
				if (dot > 0.0f)
				{
					var grass = collider.GetComponentInParent<Grass>();
					if (grass != null)
						grassList.Add(grass);
				}
			}
		}
	}
}
