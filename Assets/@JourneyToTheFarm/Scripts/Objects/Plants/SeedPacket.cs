using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.DataBase;
using JTTF.Behaviour;
using JTTF.Management;

#pragma warning disable IDE0044

namespace JTTF.Gameplay
{
	public class SeedPacket : UsableBehaviour
	{
		[Header("SeedPacket Settings")]
		[SerializeField] private string seedName = "NoName";

		private FarmPlot farmPlot = null;
		private GameObject seedPreview = null;

		public string SeedName => seedName;
		
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

			var seedAsset = GameManager.SeedDataBase.GetSeedAsset(SeedName);
			if (seedAsset != SeedAsset.None)
			{
				seedPreview = Instantiate(seedAsset.seedPreviewPrefab);
				seedPreview.SetActive(false);
			}
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
				farmPlot.SetSeed(SeedName);

				OwnerPlayer.ShortcutController.ConsumeSelectedItem();
			}
		}
	}
}
