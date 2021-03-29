using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class FarmPlot : MonoBehaviour
    {
        public bool HasSeed { get; private set; } = false;
        public bool IsMature { get; private set; } = false;

        [SerializeField] ProgressBar progressBar = null;

        GameObject seedObject = null;

        string seedName = "NoSeed";
        float growingDurationMax = 10.0f;
        float currentGrowingDuration = 0.0f;

        public float GetPercentGrowing()
		{
            return 1 - currentGrowingDuration / growingDurationMax;
		}
        public float GetTruncatePercentGrowing()
		{
            return (int)((1 - currentGrowingDuration / growingDurationMax) * 10) / 10.0f;
        }
        public void SetSeed(string name, float growingDuration)
		{
            HasSeed = true;
            seedName = name;
            growingDurationMax = growingDuration;
            currentGrowingDuration = growingDuration;

            seedObject = Instantiate(ItemList.GetSeed(seedName + "Step1").prefab);
            seedObject.transform.SetParent(transform, false);

            progressBar.SetVisible(true);
        }

        void SetSeedObject(GameObject prefab)
		{
            if (prefab == null) return;

            if (seedObject != null)
                Destroy(seedObject);

            seedObject = Instantiate(prefab);
            seedObject.transform.SetParent(transform, false);
		}
        void UpdateGrowingDuration()
		{
            if (IsMature) return;

            if (currentGrowingDuration <= 0.0f)
            {
                IsMature = true;
                progressBar.SetVisible(false);
            }
            currentGrowingDuration -= Time.deltaTime;

            progressBar.SetPercent(GetPercentGrowing());

            float percentValue = GetTruncatePercentGrowing();
            if (percentValue == 0.5f)
                SetSeedObject(ItemList.GetSeed(seedName + "Step2").prefab);
            else if (percentValue == 1.0f)
                SetSeedObject(ItemList.GetSeed(seedName + "Step3").prefab);
        }

		private void Update()
		{
            if (!HasSeed) return;

            UpdateGrowingDuration();
		}
	}
}
