using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class SeedBag : UsableItem
	{
		[Header("SeedBag parameter")]
		[SerializeField] string seedName = "NoName";
		[SerializeField] string plantName = "NoName";
		[SerializeField] float growingDuration = 0.0f;
		[SerializeField] GameObject seedPreviewPrefab = null;

		FarmPlot farmPlot = null;
		PreviewObject seedPreview = null;

		RaycastHit hit;

		void OnHasMoved(Vector3 position)
		{
			if (isUsed) return;

			if (seedPreview != null)
				if (Physics.Raycast(position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out hit))
				{
					seedPreview.transform.position = hit.point;
					seedPreview.transform.up = hit.normal;
				}

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
			if (seedPreview != null)
				Destroy(seedPreview.gameObject);

			Player.OnHasMoved -= OnHasMoved;
		}

		public override void Init(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}
		public override bool IsUsable()
		{
			var colliders = Physics.OverlapBox(seedPreview.transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.4f, 0.5f, 0.4f));
			foreach (var collider in colliders)
				if (collider.tag == "FarmPlot")
				{
					farmPlot = collider.GetComponent<FarmPlot>();
					return !farmPlot.HasSeed;
				}

			return false;
		}
		public override void ApplyEffect()
		{
			farmPlot.SetSeed(seedName, growingDuration, plantName);
		}
		public override void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterPlantAPlant(true);
		}
		public override void StopAnim(AnimationController animationController)
		{
			animationController.CharacterPlantAPlant(false);
		}
	}
}
