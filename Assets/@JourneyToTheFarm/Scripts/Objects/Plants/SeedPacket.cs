using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class SeedPacket : UsableBehaviour
	{
		[Header("SeedPacket Settings")]
		[SerializeField] SeedInfo seedInfo = SeedInfo.None;

		FarmPlot farmPlot = null;
		GameObject seedPreview = null;
		PlayerInteractionText interactionText = null;

		public SeedInfo SeedInfo => seedInfo;
		
		protected override bool CheckIsUsable()
		{
			farmPlot = OwnerPlayer.InteractableController.InteractableObject as FarmPlot;
			if (farmPlot != null)
				return !farmPlot.HasSeed;

			return false;
		}

		void UpdateFeedback()
		{
			if (IsUsable)
			{
				interactionText.SetText("Press E to Harvest");
				interactionText.SetActive(true);

				seedPreview.transform.position = farmPlot.transform.position;
				seedPreview.SetActive(true);
			}
			else
			{
				interactionText.SetActive(false);

				seedPreview.SetActive(false);
			}
		}

		protected override void Awake()
		{
			base.Awake();

			interactionText = CanvasManager.GamePanel.PlayerPanel.PlayerInteractionText;

			seedPreview = Instantiate(seedInfo.seedPreviewPrefab);
			seedPreview.SetActive(false);
		}
		protected override void Update()
		{
			base.Update();

			UpdateFeedback();
		}
		private void OnDestroy()
		{
			if (seedPreview != null)
				Destroy(seedPreview);
		}

		public override void Use()
		{
			if (farmPlot != null)
			{
				farmPlot.SetSeed(seedInfo);

				OwnerPlayer.ShortcutController.ConsumeSelectedItem();
			}
		}
	}
}
