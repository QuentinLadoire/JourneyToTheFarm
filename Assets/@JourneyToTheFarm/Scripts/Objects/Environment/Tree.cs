using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;
using JTTF.Character;
using JTTF.Inventory;
using JTTF.Management;

namespace JTTF.Gameplay
{
    public class Tree : MonoBehaviour
    {
        [SerializeField] private string logName = "Log";
		[SerializeField] private int logQuantity = 3;
		[SerializeField] private float lifeTime = 0.0f;
		[SerializeField] private GameObject modelObject = null;

		private Rigidbody rigidbodyModel = null;
		private float currentLifeTime = 0.0f;
        private bool isHarvested = false;

		private void UpdateVisibleModel()
		{
			if (isHarvested)
			{
				if (currentLifeTime <= 0.0f)
					Destroy(gameObject);
				currentLifeTime -= Time.deltaTime;
			}
		}

		private void Awake()
		{
			if (modelObject != null)
				rigidbodyModel = modelObject.GetComponent<Rigidbody>();
		}
		private void Update()
		{
			UpdateVisibleModel();
		}

		public bool IsHarvestable()
		{
            return !isHarvested;
		}
		public void Harvest(Player player)
		{
			isHarvested = true;
			currentLifeTime = lifeTime;
			rigidbodyModel.isKinematic = false;

			for (int i = 0; i < logQuantity; i++)
			{
				World.DropItem(new Item(logName, ItemType.Resource, 1), transform.position + Vector3.up * 0.5f);
			}
		}
	}
}
