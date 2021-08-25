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
		PlayerInteractionText interactionText = null;

		RaycastHit hit;

		bool IsConstructible()
		{
			var center = farmPlotPreview.transform.position + new Vector3(0.0f, 0.5f, 0.0f);
			var halfSize = new Vector3(0.4f, 0.5f, 0.4f);
			return (hit.transform != null && hit.transform.CompareTag("Constructible") &&
				!Physics.CheckBox(center, halfSize, farmPlotPreview.transform.rotation, overlapLayer));
		}
		void CheckIsUsable()
		{
			if (IsConstructible())
			{
				interactionText.SetText("Press E to Dig");
				interactionText.SetActive(true);

				isUsable = true;
			}
			else
			{
				interactionText.SetActive(false);

				isUsable = false;
			}
		}
		void UpdatePreview()
		{
			if (Physics.Raycast(OwnerPlayer.CharacterController.RoundPosition + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out hit, 5.0f, raycastLayer))
			{
				farmPlotPreview.transform.position = hit.point;
				farmPlotPreview.transform.up = hit.normal;
			}

			if (IsConstructible())
				farmPlotPreview.SetBlueColor();
			else
				farmPlotPreview.SetRedColor();
		}

		protected override void Awake()
		{
			base.Awake();

			interactionText = CanvasManager.GamePanel.PlayerPanel.PlayerInteractionText;

			farmPlotPreview = Instantiate(farmPlotPreviewPrefab).GetComponent<PreviewObject>();
		}
		private void Update()
		{
			UpdatePreview();

			CheckIsUsable();

			if (leftHandTransform != null)
				transform.up = leftHandTransform.position - transform.position;
		}
		private void OnDestroy()
		{
			if (farmPlotPreview != null)
				Destroy(farmPlotPreview.gameObject);

			if (interactionText != null)
				interactionText.SetActive(false);
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
