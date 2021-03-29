using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class SeedBag : UsableItem
	{
		[Header("SeedBag parameter")]
		[SerializeField] string seedName = "NoName";
		[SerializeField] float growingDuration = 0.0f;
		[SerializeField] GameObject seedPreviewPrefab = null;

		FarmPlot farmPlot = null;
		PreviewObject seedPreview = null;

		void OnHasMoved(Vector3 position)
		{
			if (isUsed) return;

			if (seedPreview != null)
				seedPreview.transform.position = position;

			if (IsUsable())
				seedPreview.SetBlueMat();
			else
				seedPreview.SetRedMat();
		}

		private void Awake()
		{
			seedPreview = Instantiate(seedPreviewPrefab).GetComponent<PreviewObject>();
			OnHasMoved(Player.RoundPosition);
		}
		private void Start()
		{
			Player.OnHasMoved += OnHasMoved;
		}
		private void OnDestroy()
		{
			Player.OnHasMoved -= OnHasMoved;
		}

		public override void Init(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}
		public override bool IsUsable()
		{
			var colliders = Physics.OverlapBox(seedPreview.transform.position + Vector3.up, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, LayerMask.GetMask("Farm"));
			if (colliders != null)
			{
				foreach (var collider in colliders)
					if (collider.tag == "FarmPlot")
					{
						farmPlot = collider.GetComponent<FarmPlot>();
						return !farmPlot.HasSeed;
					}
			}

			return false;
		}
		public override void ApplyEffect()
		{
			farmPlot.SetSeed(seedName, growingDuration);
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
			Destroy(seedPreview.gameObject);
		}
	}
}