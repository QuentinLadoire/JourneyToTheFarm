using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Tree : MonoBehaviour
    {
        [SerializeField] string logName = "Log";
		[SerializeField] int logQuantity = 3;
		[SerializeField] float harvestableCooldown = 0.0f;
		[SerializeField] float lifeTime = 0.0f;

		[SerializeField] GameObject modelObject = null;
		Rigidbody rigidbodyModel = null;

		float currentCooldown = 0.0f;
		float currentLifeTime = 0.0f;

        bool isHarvested = false;
		bool isDown = false;

        public bool IsHarvestable()
		{
            return !isHarvested;
		}
		public void Harvest()
		{
			isHarvested = true;
			currentCooldown = harvestableCooldown;
			currentLifeTime = lifeTime;

			Player.AddItem(ItemType.Resource, logName, logQuantity);

			rigidbodyModel.isKinematic = false;
			rigidbodyModel.AddForce(Player.Forward * 5.0f, ForceMode.Impulse);
		}

		void ResetTree()
		{
			isHarvested = false;
			isDown = false;

			rigidbodyModel.isKinematic = true;
			rigidbodyModel.transform.localPosition = Vector3.zero;
			rigidbodyModel.transform.localRotation = Quaternion.identity;

			modelObject.SetActive(true);
		}
		void UpdateCooldown()
		{
			if (currentCooldown <= 0.0f)
				ResetTree();

			currentCooldown -= Time.deltaTime;
		}
		void UpdateVisibleModel()
		{
			if (!isDown)
			{
				if (currentLifeTime <= 0.0f)
				{
					modelObject.SetActive(false);
					isDown = true;
				}
				currentLifeTime -= Time.deltaTime;
			}
		}

		void HarvestedUpdate()
		{
			if (!isHarvested) return;

			UpdateVisibleModel();

			UpdateCooldown();
		}

		private void Awake()
		{
			if (modelObject != null)
				rigidbodyModel = modelObject.GetComponent<Rigidbody>();
		}
		private void Update()
		{
			HarvestedUpdate();
		}
	}
}
