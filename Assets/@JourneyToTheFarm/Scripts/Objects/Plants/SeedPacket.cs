using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class SeedPacket : UsableBehaviour
	{
		[Header("SeedPacket Settings")]
		[SerializeField] private SeedInfo seedInfo = SeedInfo.None;

		private FarmPlot farmPlot = null;
		private GameObject seedPreview = null;

		public SeedInfo SeedInfo => seedInfo;
		
		private void UpdateFeedback()
		{
			if (IsUsable)
			{
				InteractionText.SetText("Press E to Plant");
				InteractionText.SetActive(true);

				seedPreview.transform.position = farmPlot.transform.position;
				seedPreview.SetActive(true);
			}
			else
			{
				InteractionText.SetActive(false);

				seedPreview.SetActive(false);
			}
		}

		protected override bool CheckIsUsable()
		{
			farmPlot = OwnerPlayer.InteractableController.InteractableObject as FarmPlot;
			if (farmPlot != null)
				return !farmPlot.HasSeed;

			return false;
		}

		protected override void Awake()
		{
			base.Awake();

			seedPreview = Instantiate(seedInfo.seedPreviewPrefab);
			seedPreview.SetActive(false);
		}
		protected override void Update()
		{
			base.Update();

			UpdateFeedback();
		}
		protected override void OnDestroy()
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
