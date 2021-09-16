using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;
using JTTF.Behaviour;
using JTTF.Inventory;
using JTTF.Management;
using MLAPI;
using MLAPI.Messaging;

#pragma warning disable IDE0044

namespace JTTF.Gameplay
{
    public class Tree : CustomNetworkBehaviour
    {
        [SerializeField] private string logName = "Log";
		[SerializeField] private int logQuantity = 3;
		[SerializeField] private float lifeTime = 0.0f;
		[SerializeField] private GameObject modelObject = null;

        private bool isHarvested = false;
		private TreeState treeState = null;
		private float currentLifeTime = 0.0f;
		private Rigidbody rigidbodyModel = null;

		private void OnIsHarvestedSyncValueChanged(bool previousValue, bool newValue)
		{
			rigidbodyModel.isKinematic = !newValue;
		}

		[ServerRpc(RequireOwnership = false)]
		private void HarvestServerRpc()
		{
			treeState.IsHarvestedSync.Value = true;

			HarvestSolo();
		}
		private void HarvestSolo()
		{
			isHarvested = true;
			currentLifeTime = lifeTime;
			rigidbodyModel.isKinematic = false;

			for (int i = 0; i < logQuantity; i++)
			{
				World.DropItem(new Item(logName, ItemType.Resource), transform.position + Vector3.up * 0.5f);
			}
		}

		private void UpdateVisibleModel()
		{
			if (isHarvested)
			{
				if (currentLifeTime <= 0.0f)
					Destroy(gameObject);
				currentLifeTime -= Time.deltaTime;
			}
		}

		private void UpdateSolo()
		{
			UpdateVisibleModel();
		}
		private void UpdateMulti()
		{
			if (!NetworkManager.Singleton.IsServer) return;

			UpdateVisibleModel();
		}

		private bool IsHarvestableSolo()
		{
			return !isHarvested;
		}
		private bool IsHarvestableMulti()
		{
			return !treeState.IsHarvestedSync.Value;
		}

		protected override void Awake()
		{
			base.Awake();

			if (modelObject != null)
				rigidbodyModel = modelObject.GetComponent<Rigidbody>();

			treeState = GetComponent<TreeState>();
		}
		protected override void Start()
		{
			base.Start();

			if (GameManager.IsMulti && NetworkManager.Singleton.IsClient)
			{
				treeState.IsHarvestedSync.OnValueChanged += OnIsHarvestedSyncValueChanged;
			}
		}
		protected override void Update()
		{
			base.Update();

			if (GameManager.IsMulti)
				UpdateMulti();
			else
				UpdateSolo();
		}

		public bool IsHarvestable()
		{
			if (GameManager.IsMulti)
				return IsHarvestableMulti();
			else
				return IsHarvestableSolo();
		}
		public void Harvest()
		{
			if (GameManager.IsMulti)
				HarvestServerRpc();
			else
				HarvestSolo();
		}
	}
}
