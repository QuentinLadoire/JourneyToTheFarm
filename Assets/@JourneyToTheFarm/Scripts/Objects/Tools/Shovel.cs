using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Shovel : CustomBehaviour, IEquipable, IUsable
	{
		public float Duration => duration;
		public ActionType ActionType => ActionType.Dig;

		[Header("Shovel Parameters")]
		public float duration = 0.0f;
		public GameObject farmPlotPrefab = null;
		public GameObject farmPlotPreviewPrefab = null;

		Transform leftHandTransform = null;
		PreviewObject farmPlotPreview = null;

		RaycastHit hit;

		public void Equip(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
			leftHandTransform = leftHand;
		}

		public bool IsUsable()
		{
			return IsConstructible();
		}
		public void Use()
		{
			var farmPlot = Instantiate(farmPlotPrefab);
			farmPlot.transform.position = farmPlotPreview.transform.position;
			farmPlot.transform.up = farmPlotPreview.transform.up;
		}

		bool IsConstructible()
		{
			var center = farmPlotPreview.transform.position + new Vector3(0.0f, 0.5f, 0.0f);
			var halfSize = new Vector3(0.4f, 0.5f, 0.4f);
			return (hit.transform != null && hit.transform.CompareTag("Constructible") &&
				!Physics.CheckBox(center, halfSize, farmPlotPreview.transform.rotation, GameManager.GetConstructibleOverlapMask()));
		}
		void UpdatePreview()
		{
			if (Physics.Raycast(Player.RoundPosition + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out hit, 5.0f, GameManager.GetConstructiblRaycastMask()))
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

			farmPlotPreview = Instantiate(farmPlotPreviewPrefab).GetComponent<PreviewObject>();
		}
		private void Update()
		{
			UpdatePreview();

			if (leftHandTransform != null)
				transform.up = leftHandTransform.position - transform.position;
		}
		private void OnDestroy()
		{
			if (farmPlotPreview != null)
				Destroy(farmPlotPreview.gameObject);
		}
	}
}
