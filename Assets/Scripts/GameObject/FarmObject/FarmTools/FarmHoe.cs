using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmHoe : SimpleObject, IHandable, IUsable
	{
		public float Duration => duration;
		public float AnimationDuration => animationDuration;
		public float AnimationMultiplier => animationMultiplier;

		[Header("Animation Parameter")]
		[SerializeField] float duration = 0.0f;
		[SerializeField] float animationDuration = 0.0f;
		[SerializeField] float animationMultiplier = 1.0f;

		[Header("FarmHoe Parameter")]
		[SerializeField] GameObject farmPlotPrefab = null;
		[SerializeField] GameObject farmPlotPreviewPrefab = null;

		Transform leftHandTransform = null;
		PreviewObject farmPlotPreview = null;

		RaycastHit hit;

		public void SetHanded(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
			leftHandTransform = leftHand;
		}

		public bool IsUsable()
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
		public void Use()
		{
			var farmPlot = Instantiate(farmPlotPrefab);
			farmPlot.transform.position = farmPlotPreview.transform.position;
		}

		public void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterDiggingAnim(true, animationController.GetDesiredAnimationSpeed(duration, animationDuration, animationMultiplier));
		}
		public void StopAnim(AnimationController animationController)
		{
			animationController.CharacterDiggingAnim(false);
		}

		void OnHasMoved()
		{
			if (Physics.Raycast(Player.RoundPosition + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out hit))
			{
				farmPlotPreview.transform.position = hit.point;
				farmPlotPreview.transform.up = hit.normal;
			}

			if (IsUsable())
				farmPlotPreview.SetBlueColor();
			else
				farmPlotPreview.SetRedColor();
		}

		private void Awake()
		{
			farmPlotPreview = Instantiate(farmPlotPreviewPrefab).GetComponent<PreviewObject>();
			OnHasMoved();
		}
		private void Start()
		{
			Player.OnHasMoved += OnHasMoved;
		}
		private void Update()
		{
			if (leftHandTransform != null)
				transform.up = leftHandTransform.position - transform.position;
		}
		private void OnDestroy()
		{
			if (farmPlotPreview != null)
				Destroy(farmPlotPreview.gameObject);

			Player.OnHasMoved -= OnHasMoved;
		}
	}
}
