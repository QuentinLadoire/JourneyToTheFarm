using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Shovel : UsableBehaviour
	{
		[Header("Shovel Settings")]
		[SerializeField] LayerMask raycastLayer = -1;
		[SerializeField] LayerMask overlapLayer = -1;
		[SerializeField] GameObject farmPlotPrefab = null;
		[SerializeField] GameObject farmPlotPreviewPrefab = null;

		Transform leftHandTransform = null;
		PreviewObject farmPlotPreview = null;
		
		private bool IsConstructible()
		{
			if (Physics.Raycast(OwnerPlayer.CharacterController.RoundPosition + Vector3.up, Vector3.down, out RaycastHit hit, 5.0f, raycastLayer))
			{
				farmPlotPreview.transform.position = hit.point;
				farmPlotPreview.transform.up = hit.normal;

				var center = farmPlotPreview.transform.position + new Vector3(0.0f, 0.5f, 0.0f);
				var halfSize = new Vector3(0.4f, 0.5f, 0.4f);
				return (hit.transform != null && hit.transform.CompareTag("Constructible") &&
					!Physics.CheckBox(center, halfSize, farmPlotPreview.transform.rotation, overlapLayer));
			}

			return false;
		}
		private void UpdateFeedback()
		{
			if (IsUsable)
			{
				farmPlotPreview.SetBlueColor();

				InteractionText.SetText("Press E to Dig");
				InteractionText.SetActive(true);
			}
			else
			{
				farmPlotPreview.SetRedColor();

				InteractionText.SetActive(false);
			}
		}

		protected override bool CheckIsUsable()
		{
			return IsConstructible();
		}

		protected override void Start()
		{
			base.Start();

			farmPlotPreview = Instantiate(farmPlotPreviewPrefab).GetComponent<PreviewObject>();
		}
		protected override void Update()
		{
			base.Update();

			UpdateFeedback();

			if (leftHandTransform != null)
				transform.up = leftHandTransform.position - transform.position;
		}
		protected override void OnDestroy()
		{
			if (farmPlotPreview != null)
				Destroy(farmPlotPreview.gameObject);
		}

		public override void Equip(Transform rightHand, Transform leftHand)
		{
			base.Equip(rightHand, leftHand);

			leftHandTransform = leftHand;
		}
		public override void Use()
		{
			var farmPlot = Instantiate(farmPlotPrefab);
			farmPlot.transform.position = farmPlotPreview.transform.position;
			farmPlot.transform.up = farmPlotPreview.transform.up;
		}
	}
}
