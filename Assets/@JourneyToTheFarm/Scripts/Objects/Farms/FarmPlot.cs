using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

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

        private FarmPlotState farmPlotState = null;

        public bool HasSeed => seedAsset != SeedAsset.None;
        public bool IsMature => HasSeed && currentGrowDuration <= 0.0f;

        private void OnSeedNameSyncValueChanged(string previousValue, string newValue)
		{
            if (newValue == "NoName")
			{
                ClearSeed();
            }
            else
			{
                seedAsset = GameManager.SeedDataBase.GetSeedAsset(newValue);
            }
		}

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

            if (GameManager.IsMulti)
			{
                farmPlotState.SeedNameSync.Value = seedAsset.name;
                farmPlotState.CurrentGrowDurationSync.Value = currentGrowDuration;
			}
		}
        private void UpdateGrowing()
		{
            if (!IsMature)
            {
                if (!(currentGrowDuration <= 0.0f))
                {
                    currentGrowDuration -= Time.deltaTime;

                    if (GameManager.IsMulti)
                        farmPlotState.CurrentGrowDurationSync.Value = currentGrowDuration;
                }
            }
		}
        private void UpdateSeedObject(float percent)
		{
            if (seedAsset == SeedAsset.None) return;

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

		protected override void Awake()
		{
            base.Awake();

            farmPlotState = GetComponent<FarmPlotState>();
        }
        protected override void Start()
		{
            base.Start();

            if (GameManager.IsMulti && NetworkManager.Singleton.IsClient)
			{
                farmPlotState.SeedNameSync.OnValueChanged += OnSeedNameSyncValueChanged;
            }
		}
		protected override void Update()
		{
            base.Update();

            if (GameManager.IsMulti)
            {
                if (NetworkManager.Singleton.IsServer)
				{
                    UpdateGrowing();
                }

                if (NetworkManager.Singleton.IsClient)
				{
                    currentGrowDuration = farmPlotState.CurrentGrowDurationSync.Value;
				}

                UpdateFeedback();
            }
            else
			{
                UpdateGrowing();
                UpdateFeedback();
            }
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

            if (GameManager.IsMulti && NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient)
			{
                farmPlotState.SeedNameSync.OnValueChanged -= OnSeedNameSyncValueChanged;
			}
		}

		public void SetSeed(string seedName)
		{
            seedAsset = GameManager.SeedDataBase.GetSeedAsset(seedName);
            if (seedAsset != SeedAsset.None)
            {
                currentGrowDuration = seedAsset.growDuration;

                if (GameManager.IsMulti)
				{
                    farmPlotState.SeedNameSync.Value = seedAsset.name;
                    farmPlotState.CurrentGrowDurationSync.Value = currentGrowDuration;
				}
            }
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
