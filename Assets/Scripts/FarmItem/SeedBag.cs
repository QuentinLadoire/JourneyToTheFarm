using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class SeedBag : UsableItem
	{
		[Header("SeedBag parameter")]
		[SerializeField] SeedInfo seedInfo = null;

		FarmPlot farmPlot = null;

		void OnHasMoved(Vector3 position)
		{
			if (isUsed) return;

			if (IsUsable())
			{

			}
		}

		private void Start()
		{
			Player.OnHasMoved += OnHasMoved;
		}
		private void OnDestroy()
		{
			Player.OnHasMoved -= OnHasMoved;
		}

		public override void Init(Transform parent)
		{
			transform.SetParent(parent, false);
		}
		public override bool IsUsable()
		{
			var colliders = Physics.OverlapSphere(Player.Position, 0.1f);
			if (colliders != null)
			{
				foreach (var collider in colliders)
					if (collider.tag == "FarmPlot")
					{
						farmPlot = collider.GetComponent<FarmPlot>();
						return true;
					}
			}

			return false;
		}
		public override void ApplyEffect()
		{
			Debug.Log("ApplyEffect not implemented");
		}
		public override void PlayAnim(AnimationController animationController)
		{
			Debug.Log("PlayAnim not implemented");
		}
		public override void StopAnim(AnimationController animationController)
		{
			Debug.Log("StopAnim not implemented");
		}
		public override void Destroy()
		{
			Destroy(gameObject);
		}
	}
}
