using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.UI;
using JTTF.Enum;
using JTTF.DataBase;
using JTTF.Behaviour;
using JTTF.Character;
using JTTF.Inventory;
using JTTF.Management;
using MLAPI;
using MLAPI.Messaging;

namespace JTTF.Gameplay
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

        [ServerRpc(RequireOwnership = false)]
        private void ClearSeedServerRpc()
        {
            currentGrowDuration = 0.0f;
            seedAsset = SeedAsset.None;

            farmPlotState.SeedNameSync.Value = "NoName";
            farmPlotState.AlreadyInInteraction.Value = false;
            farmPlotState.CurrentGrowDurationSync.Value = 0.0f;
        }
        private void ClearSeedSolo()
        {
            currentIndex = -1;
            currentGrowDuration = 0.0f;
            seedAsset = SeedAsset.None;

            Destroy(seedObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetSeedServerRpc(string seedName)
        {
            seedAsset = GameManager.SeedDataBase.GetSeedAsset(seedName);
            if (seedAsset != SeedAsset.None)
            {
                farmPlotState.SeedNameSync.Value = seedName;
                farmPlotState.CurrentGrowDurationSync.Value = seedAsset.growDuration;
            }
        }
        private void SetSeedSolo(string seedName)
        {
            seedAsset = GameManager.SeedDataBase.GetSeedAsset(seedName);
            if (seedAsset != SeedAsset.None)
            {
                currentGrowDuration = seedAsset.growDuration;
            }
        }

        private void OnSeedNameSyncValueChanged(string previousValue, string newValue)
		{
            if (newValue == "NoName")
                ClearSeedSolo();
            else
                SetSeedSolo(newValue);
		}
        private void OnCurrentGrowDurationSynValueChanged(float previousValue, float newValue)
		{
            currentGrowDuration = newValue;
		}

        private void DropPlant(Player player)
		{
            if (World.DropItem(new Item(seedAsset.name, ItemType.Resource, 1), transform.position) == null)
                player.AddItem(new Item(seedAsset.name, ItemType.Resource, 1));
        }
        private void ClearSeed()
		{
            if (GameManager.IsMulti)
                ClearSeedServerRpc();
            else
                ClearSeedSolo();
		}

        private void UpdateGrowing()
		{
            if (!IsMature)
            {
                if (currentGrowDuration > 0.0f)
                    currentGrowDuration -= Time.deltaTime;
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
            else if (HasSeed)
			{
                var currentPercent = 1 - (currentGrowDuration / seedAsset.growDuration);

                progressBar.SetActive(true);
                progressBar.SetPercent(currentPercent);

                UpdateSeedObject(currentPercent);
			}
            else
			{
                progressBar.SetActive(false);
                InteractableImage.SetActive(false);
            }
		}

        private void UpdateSolo()
		{
            UpdateGrowing();
            UpdateFeedback();
        }
        private void UpdateMulti()
		{
            if (NetworkManager.Singleton.IsServer)
            {
                if (!IsMature)
                {
                    if (currentGrowDuration > 0.0f)
                    {
                        currentGrowDuration -= Time.deltaTime;

                        farmPlotState.CurrentGrowDurationSync.Value = currentGrowDuration;
                    }
                }
            }

            if (NetworkManager.Singleton.IsClient)
            {
                UpdateFeedback();
            }
        }

        private bool CheckIsInteractableSolo()
		{
            return IsMature;
		}
        private bool CheckIsInteractbleMulti()
		{
            return !farmPlotState.AlreadyInInteraction.Value && IsMature;
		}
        protected override bool CheckIsInteractable()
		{
            if (GameManager.IsMulti)
                return CheckIsInteractbleMulti();
            else
                return CheckIsInteractableSolo();
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
                farmPlotState.CurrentGrowDurationSync.OnValueChanged += OnCurrentGrowDurationSynValueChanged;
			}
		}
		protected override void Update()
		{
            base.Update();

            if (GameManager.IsMulti)
            {
                UpdateMulti();
            }
            else
			{
                UpdateSolo();
            }
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

            if (GameManager.IsMulti && NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient)
			{
                farmPlotState.SeedNameSync.OnValueChanged -= OnSeedNameSyncValueChanged;
                farmPlotState.CurrentGrowDurationSync.OnValueChanged -= OnCurrentGrowDurationSynValueChanged;
            }
		}

		public void SetSeed(string seedName)
		{
            if (GameManager.IsMulti)
                SetSeedServerRpc(seedName);
            else
                SetSeedSolo(seedName);
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

		public override void StartToInteract()
		{
            if (GameManager.IsMulti)
			{
                farmPlotState.AlreadyInInteraction.Value = true;
			}
		}
		public override void Interact(Player player)
        {
            DropPlant(player);

            ClearSeed();
        }
    }
}
