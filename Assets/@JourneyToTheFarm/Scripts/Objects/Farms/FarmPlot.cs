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
using MLAPI.NetworkVariable;

#pragma warning disable IDE0044
#pragma warning disable IDE0090

namespace JTTF.Gameplay
{
    public class FarmPlot : InteractableBehaviour
    {
        [Header("FarmPlot Settings")]
        [SerializeField] private FarmPlotProgressBar progressBar = null;

        private int currentIndex = -1;
        private GameObject seedObject = null;
        private SeedAsset seedAsset = SeedAsset.None;

        private NetworkVariableString seedName = new NetworkVariableString(new NetworkVariableSettings
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.ServerOnly
        });
        private NetworkVariableBool alreadyInInteraction = new NetworkVariableBool(new NetworkVariableSettings
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.Everyone
        });
        private NetworkVariableFloat currentGrowDuration = new NetworkVariableFloat(new NetworkVariableSettings
        {
            ReadPermission = NetworkVariablePermission.Everyone,
            WritePermission = NetworkVariablePermission.ServerOnly
        }, 0.0f);

        public bool HasSeed => seedAsset != SeedAsset.None;
        public bool IsMature => HasSeed && currentGrowDuration.Value <= 0.0f;

        [ServerRpc(RequireOwnership = false)]
        private void ClearSeedServerRpc()
        {
            ClearSeedSolo();
        }
        private void ClearSeedSolo()
        {
            currentIndex = -1;
            seedAsset = SeedAsset.None;
            seedName.Value = "NoName";
            currentGrowDuration.Value = 0.0f;
            alreadyInInteraction.Value = false;

            Destroy(seedObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetSeedServerRpc(string seedName)
        {
            SetSeedSolo(seedName);
        }
        private void SetSeedSolo(string seedName)
        {
            seedAsset = GameManager.SeedDataBase.GetSeedAsset(seedName);
            if (seedAsset != SeedAsset.None)
            {
                this.seedName.Value = seedAsset.name;

                if (!GameManager.IsMulti || NetworkManager.Singleton.IsServer)
                    currentGrowDuration.Value = seedAsset.growDuration;
            }
        }

        private void OnSeedNameValueChanged(string previousValue, string newValue)
		{
            if (newValue == "NoName")
                ClearSeedSolo();
            else
                SetSeedSolo(newValue);
		}

        private void DropPlant(Player player)
		{
            //Spawn a Collectible in term
            //World.DropItem(new Item(seedAsset.name, ItemType.Resource, 1), transform.position)
            player.AddItem(new Item(seedAsset.name, ItemType.Resource), 1);
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
                if (currentGrowDuration.Value > 0.0f)
                    currentGrowDuration.Value -= Time.deltaTime;
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
                var currentPercent = 1 - (currentGrowDuration.Value / seedAsset.growDuration);

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
                UpdateGrowing();
            }

            if (NetworkManager.Singleton.IsClient)
            {
                UpdateFeedback();
            }
        }

        protected override bool CheckIsInteractable()
		{
            return !alreadyInInteraction.Value && IsMature;
        }

		protected override void Start()
		{
			base.Start();

            if (GameManager.IsMulti && NetworkManager.Singleton.IsClient)
			{
                seedName.OnValueChanged += OnSeedNameValueChanged;
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
                alreadyInInteraction.Value = true;
			}
		}
		public override void Interact(Player player)
        {
            DropPlant(player);

            ClearSeed();
        }
        public override void StopToInteract()
		{
            InteractionText.SetActive(false);
		}
    }
}
