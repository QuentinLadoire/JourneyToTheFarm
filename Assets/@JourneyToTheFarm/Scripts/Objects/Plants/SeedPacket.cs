using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	[System.Serializable]
	public struct SeedInfo
	{
		public static SeedInfo None = new SeedInfo("NoName", 0.0f, null, null);

		public string name;
		public float growDuration;
		public GameObject seedPreviewPrefab;
		public GameObject[] seedStepPrefabs;

		public SeedInfo(string name, float growDuration, GameObject seedPreviewPrefab, GameObject[] seedStepPrefabs)
		{
			this.name = name;
			this.growDuration = growDuration;
			this.seedPreviewPrefab = seedPreviewPrefab;
			this.seedStepPrefabs = seedStepPrefabs;
		}
	}

	public class SeedPacket : CustomBehaviour, IEquipable, IOwnable, IUsable
	{
		[SerializeField] float duration = 1.0f;
		[SerializeField] SeedInfo seedInfo = SeedInfo.None;

		bool isUsable = false;
		FarmPlot farmPlot = null;
		GameObject seedPreview = null;
		PlayerInteractionText interactionText = null;

		public bool IsUsable => isUsable;
		public SeedInfo SeedInfo => seedInfo;
		public float Duration => duration;
		public ActionType ActionType => ActionType.Plant;
		public Player OwnerPlayer { get; private set; }

		void CheckIsUsable()
		{
			var tmp = OwnerPlayer.InteractableController.InteractableObject as FarmPlot;
			if (tmp != farmPlot)
			{
				farmPlot = tmp;
				isUsable = false;
				seedPreview.SetActive(false);
				interactionText.SetActive(false);

				if (farmPlot != null && !farmPlot.HasSeed)
				{
					seedPreview.transform.position = farmPlot.transform.position;
					seedPreview.SetActive(true);

					interactionText.SetText("Press E to Plant");
					interactionText.SetActive(true);

					isUsable = true;
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();

			interactionText = CanvasManager.GamePanel.PlayerPanel.PlayerInteractionText;

			seedPreview = Instantiate(seedInfo.seedPreviewPrefab);
			seedPreview.SetActive(false);
		}
		private void Update()
		{
			CheckIsUsable();
		}
		private void OnDestroy()
		{
			if (seedPreview != null)
				Destroy(seedPreview);
		}

		public void SetOwner(Player owner)
		{
			OwnerPlayer = owner;
		}
		public void Equip(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}
		public void Use()
		{
			if (farmPlot != null)
			{
				farmPlot.SetSeed(seedInfo);

				isUsable = false;
				seedPreview.SetActive(false);
				interactionText.SetActive(false);

				OwnerPlayer.ShortcutController.ConsumeSelectedItem();
			}
		}
	}
}
