using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class FarmPlot : CustomBehaviour, IInteractable
    {
        [Header("FarmPlot Parameters")]
        [SerializeField] float duration = 0.0f;
        [SerializeField] GameObject activableImage = null;
        [SerializeField] FarmPlotProgressBar progressBar = null;

        int currentIndex = -1;
        GameObject seedObject = null;
        float currentGrowDuration = 0.0f;
        SeedInfo seedInfo = SeedInfo.None;

        public bool HasSeed { get; private set; } = false;
        public bool IsMature { get; private set; } = false;

        public float Duration => duration;
        public ActionType ActionType => ActionType.Pick;

		private void Update()
		{
			if (HasSeed && !IsMature)
			{
                if (currentGrowDuration <= 0.0f)
                {
                    IsMature = true;
                    progressBar.SetActive(false);
                    activableImage.SetActive(true);
                }
                currentGrowDuration -= Time.deltaTime;

                var currentPercent = 1 - (currentGrowDuration / seedInfo.growDuration);
                var index = (int)((seedInfo.seedStepPrefabs.Length - 1) * currentPercent);

                progressBar.SetPercent(currentPercent);
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
		}

		public void SetSeed(SeedInfo seedInfo)
		{
            this.seedInfo = seedInfo;

            HasSeed = true;
            currentIndex = -1;
            currentGrowDuration = seedInfo.growDuration;

            progressBar.SetActive(true);
            progressBar.SetPercent(1.0f);
            activableImage.SetActive(false);
        }

		public void Select()
		{
            activableImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		}
		public void Deselect()
		{
            activableImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}

		public bool IsInteractable()
		{
            return IsMature;
		}
        public void StartToInteract()
        {
            //nothing here
        }
        public void Interact(Player player)
        {
            if (World.DropItem(new Item(seedInfo.name, ItemType.Resource, 1), transform.position) == null)
			{
                if (!player.ShortcutController.AddItem(new Item(seedInfo.name, ItemType.Resource, 1)))
				{
                    player.InventoryController.AddItem(new Item(seedInfo.name, ItemType.Resource, 1));
				}
			}

            activableImage.SetActive(false);

            currentIndex = -1;
            if (seedObject != null) Destroy(seedObject);
            currentGrowDuration = 0.0f;
            seedInfo = SeedInfo.None;

            IsMature = false;
            HasSeed = false;
        }
    }
}
