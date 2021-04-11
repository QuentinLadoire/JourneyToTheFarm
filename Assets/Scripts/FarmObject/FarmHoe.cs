using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmHoe : UsableObject
	{
		[Header("FarmHoe Parameter")]
		[SerializeField] GameObject farmPlotPrefab = null;
		[SerializeField] GameObject farmPlotPreviewPrefab = null;

		PreviewObject farmPlotPreview = null;
		Transform leftHandTransform = null;

		RaycastHit hit;

		void OnHasMoved(Vector3 position)
		{
			if (isUsed) return;

			if (Physics.Raycast(position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out hit))
			{
				farmPlotPreview.transform.position = hit.point;
				farmPlotPreview.transform.up = hit.normal;
			}

			if (IsUsable())
				farmPlotPreview.SetBlueMat();
			else
				farmPlotPreview.SetRedMat();
		}

		private void Awake()
		{
			farmPlotPreview = Instantiate(farmPlotPreviewPrefab).GetComponent<PreviewObject>();
			OnHasMoved(Player.RoundPosition);
		}
		private void Start()
		{
			Player.OnHasMoved += OnHasMoved;
		}
		private void Update()
		{
			transform.up = leftHandTransform.position - transform.position;
		}
		private void OnDestroy()
		{
			if (farmPlotPreview != null)
				Destroy(farmPlotPreview.gameObject);

			Player.OnHasMoved -= OnHasMoved;
		}

		public override void Init(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
			leftHandTransform = leftHand;
		}
		public override bool IsUsable()
		{
			if (hit.collider != null && hit.collider.CompareTag("Ground"))
			{
				var colliders = Physics.OverlapBox(farmPlotPreview.transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.4f, 0.5f, 0.4f));
				foreach (var collider in colliders)
					if (collider.CompareTag("Obstacle") || collider.CompareTag("FarmPlot"))
						return false;

				return true;
			}

			return false;
		}
		public override void ApplyEffect()
		{
			var farmPlot = Instantiate(farmPlotPrefab);
			farmPlot.transform.position = farmPlotPreview.transform.position;
		}
		public override void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterDiggingAnim(true, GetDesiredAnimationSpeed());
		}
		public override void StopAnim(AnimationController animationController)
		{
			animationController.CharacterDiggingAnim(false);
		}
	}
}
