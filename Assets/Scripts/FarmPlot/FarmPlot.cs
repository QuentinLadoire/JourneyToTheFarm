using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class FarmPlot : ActivableObject
    {
        public bool HasSeed { get; private set; } = false;
        public bool IsMature { get; private set; } = false;

        [SerializeField] ProgressBar progressBar = null;
        [SerializeField] GameObject activableImage = null;

        GameObject seedObject = null;

        string seedName = "NoSeed";
        string plantName = "NoName";
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
        public void SetSeed(string seedName, float growingDuration, string plantName)
		{
            HasSeed = true;
            this.seedName = seedName;
            this.plantName = plantName;
            growingDurationMax = growingDuration;
            currentGrowingDuration = growingDuration;

			seedObject = Instantiate(GameManager.DataBase.GetSeed(this.seedName + "Step1").prefab);
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
                activableImage.SetActive(true);
            }
            currentGrowingDuration -= Time.deltaTime;

            progressBar.SetPercent(GetPercentGrowing());

            float percentValue = GetTruncatePercentGrowing();
            if (percentValue == 0.5f)
                SetSeedObject(GameManager.DataBase.GetSeed(seedName + "Step2").prefab);
            else if (percentValue == 1.0f)
                SetSeedObject(GameManager.DataBase.GetSeed(seedName + "Step3").prefab);
        }

		private void Update()
		{
            if (!HasSeed) return;

            UpdateGrowingDuration();
		}

		public override void Select()
		{
            activableImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		}
		public override void Deselect()
		{
            activableImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}
		public override bool IsActivable()
		{
            return IsMature;
		}
        public override void ApplyEffect()
		{
            Player.AddItem(ItemType.Plant, plantName);

            IsMature = false;
            activableImage.SetActive(false);

            seedName = "NoSeed";
            plantName = "NoName";
            HasSeed = false;

            if (seedObject != null)
                Destroy(seedObject);
		}
        public override void PlayAnim(AnimationController animationController)
		{
            animationController.CharacterPickUp(true, GetDesiredAnimationSpeed());
		}
        public override void StopAnim(AnimationController animationController)
		{
            animationController.CharacterPickUp(false);
        }
    }
}
