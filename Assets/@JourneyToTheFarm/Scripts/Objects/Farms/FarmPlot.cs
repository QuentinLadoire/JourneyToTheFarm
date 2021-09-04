using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class FarmPlot : InteractableBehaviour
    {
        [Header("FarmPlot Settings")]
        [SerializeField] private FarmPlotProgressBar progressBar = null;

        private int currentIndex = -1;
        private GameObject seedObject = null;
        private float currentGrowDuration = 0.0f;
        private SeedInfo seedInfo = SeedInfo.None;

        public bool HasSeed => seedInfo != SeedInfo.None;
        public bool IsMature => HasSeed && currentGrowDuration <= 0.0f;

        private void DropPlant(Player player)
		{
            if (World.DropItem(new Item(seedInfo.name, ItemType.Resource, 1), transform.position) == null)
            {
                if (!player.ShortcutController.AddItem(new Item(seedInfo.name, ItemType.Resource, 1)))
                {
                    player.InventoryController.AddItem(new Item(seedInfo.name, ItemType.Resource, 1));
                }
            }
        }
        private void ClearSeed()
		{
            currentIndex = -1;
            currentGrowDuration = 0.0f;
            seedInfo = SeedInfo.None;

            Destroy(seedObject);
		}
        private void UpdateGrowing()
		{
            if (!IsMature)
            {
                if (!(currentGrowDuration <= 0.0f))
                    currentGrowDuration -= Time.deltaTime;
            }
		}
        private void UpdateSeedObject(float percent)
		{
            var index = (int)((seedInfo.seedStepPrefabs.Length - 1) * percent);
            if (index != currentIndex)
            {
                if (seedObject != null)
                    Destroy(seedObject);

                seedObject = Instantiate(seedInfo.seedStepPrefabs[index]);
                seedObject.transform.position = Vector3.zero;
                seedObject.transform.SetParent(transform, false);

                currentIndex = index;
            }
        }
        private void UpdateFeedback()
		{
            if (IsInteractable)
			{
                progressBar.SetActive(false);

                InteractableImage.SetActive(true);
			}
            else
			{
                if (HasSeed)
				{
                    var currentPercent = 1 - (currentGrowDuration / seedInfo.growDuration);

                    progressBar.SetActive(true);
                    progressBar.SetPercent(currentPercent);

                    UpdateSeedObject(currentPercent);
                }

                InteractableImage.SetActive(false);
			}
		}

		protected override bool CheckIsInteractable()
		{
            return IsMature;
		}

		protected override void Update()
		{
            base.Update();

            UpdateGrowing();
            UpdateFeedback();
		}

		public void SetSeed(SeedInfo seedInfo)
		{
            this.seedInfo = seedInfo;
            currentGrowDuration = seedInfo.growDuration;
        }

		public override void Select()
		{
			base.Select();

            InteractionText.SetText("Press E to Gather");
            InteractionText.SetActive(true);
        }
		public override void Deselect()
		{
			base.Deselect();

            InteractionText.SetActive(false);
		}

		public override void Interact(Player player)
        {
            DropPlant(player);
            
            ClearSeed();
        }
    }
}
