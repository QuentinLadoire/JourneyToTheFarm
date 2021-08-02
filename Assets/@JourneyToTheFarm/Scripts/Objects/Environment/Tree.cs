using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Tree : MonoBehaviour
    {
        public string logName = "Log";
		public int logQuantity = 3;
		public float lifeTime = 0.0f;
		public GameObject modelObject = null;

		Rigidbody rigidbodyModel = null;
		float currentLifeTime = 0.0f;
        bool isHarvested = false;

        public bool IsHarvestable()
		{
            return !isHarvested;
		}
		public void Harvest()
		{
			isHarvested = true;
			currentLifeTime = lifeTime;
			rigidbodyModel.isKinematic = false;

			//Player.AddItem(new ItemInfo(logName, ItemType.Resource, logQuantity));
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
