using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable IDE0044

namespace JTTF
{
    public class FarmPlot : InteractableBehaviour
    {
        [Header("FarmPlot Settings")]
        [SerializeField] private FarmPlotProgressBar progressBar = null;

        private int currentIndex = -1;
        private GameObject seedObject = null;
        private float currentGrowDuration = 0.0f;
        private SeedAsset seedAsset = SeedAsset.None;

        public bool HasSeed => seedAsset != SeedAsset.None;
        public bool IsMature => HasSeed && currentGrowDuration <= 0.0f;

        private void DropPlant(Player player)
		{
            if (World.DropItem(new Item(seedAsset.name, ItemType.Resource, 1), transform.position) == null)
                player.AddItem(new Item(seedAsset.name, ItemType.Resource, 1));
        }
        private void ClearSeed()
		{
            currentIndex = -1;
            currentGrowDuration = 0.0f;
            seedAsset = SeedAsset.None;

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
            var index = (int)((seedAsset.seedStepPrefabs.Length - 1) * percent);
            if (index != currentIndex)
            {
                if (seedObject != null)
                    Destroy(seedObject);

                seedObject = Instantiate(seedAsset.seedStepPrefabs[index]);
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
                    var currentPercent = 1 - (currentGrowDuration / seedAsset.growDuration);

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

            if (GameManager.IsMulti)
            {
                if (NetworkManager.IsServer)
				{
                    UpdateGrowing();
                }

                UpdateFeedback();
            }
            else
			{
                UpdateGrowing();
                UpdateFeedback();
            }
		}

		public void SetSeed(string seedName)
		{
            seedAsset = GameManager.SeedDataBase.GetSeedAsset(seedName);
            if (seedAsset != SeedAsset.None)
                currentGrowDuration = seedAsset.growDuration;
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
