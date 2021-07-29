using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Rock : MonoBehaviour
    {
		[SerializeField] string stoneName = "Stone";
		[SerializeField] int stoneQuantity = 3;
		[SerializeField] float harvestableCooldown = 0.0f;

		[SerializeField] GameObject modelObject = null;

		float currentCooldown = 0.0f;

		bool isHarvested = false;

		public bool IsHarvestable()
		{
			return !isHarvested;
		}
		public void Harvest()
		{
			isHarvested = true;
			currentCooldown = harvestableCooldown;

			Player.AddItem(new ItemInfo(stoneName, ItemType.Resource, stoneQuantity));

			modelObject.SetActive(false);
		}

		void ResetRock()
		{
			isHarvested = false;

			modelObject.SetActive(true);
		}
		void UpdateCooldown()
		{
			if (!isHarvested) return;

			if (currentCooldown <= 0.0f)
				ResetRock();

			currentCooldown -= Time.deltaTime;
		}

		private void Update()
		{
			UpdateCooldown();
		}
	}
}
