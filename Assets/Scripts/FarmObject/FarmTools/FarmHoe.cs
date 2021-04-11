using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmHoe : ActivableObject
	{
		[Header("FarmHoe Parameter")]
		[SerializeField] GameObject farmPlotPrefab = null;
		[SerializeField] GameObject farmPlotPreviewPrefab = null;

		TransportableObject transportableObject = null;
		Transform leftHandTransform = null;

		PreviewObject farmPlotPreview = null;

		RaycastHit hit;

		void OnSetHands(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
			leftHandTransform = leftHand;
		}
		void OnHasMoved(Vector3 position)
		{
			if (isActived) return;

			if (Physics.Raycast(position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out hit))
			{
				farmPlotPreview.transform.position = hit.point;
				farmPlotPreview.transform.up = hit.normal;
			}

			if (IsActivable())
				farmPlotPreview.SetBlueMat();
			else
				farmPlotPreview.SetRedMat();
		}

		private void Awake()
		{
			transportableObject = GetComponent<TransportableObject>();
			transportableObject.onSetHands += OnSetHands;

			farmPlotPreview = Instantiate(farmPlotPreviewPrefab).GetComponent<PreviewObject>();
			OnHasMoved(Player.RoundPosition);
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

			transportableObject.onSetHands -= OnSetHands;
			Player.OnHasMoved -= OnHasMoved;
		}

		public override bool IsActivable()
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
