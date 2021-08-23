using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Tree : MonoBehaviour
    {
        [SerializeField] string logName = "Log";
		[SerializeField] int logQuantity = 3;
		[SerializeField] float lifeTime = 0.0f;
		[SerializeField] GameObject modelObject = null;

		Rigidbody rigidbodyModel = null;
		float currentLifeTime = 0.0f;
        bool isHarvested = false;

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

		void UpdateVisibleModel()
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
	}
}
