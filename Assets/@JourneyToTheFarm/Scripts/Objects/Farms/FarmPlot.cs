using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class FarmPlot : CustomBehaviour, IInteractable
    {
        public float Duration => duration;
        public float AnimationDuration => animationDuration;
        public float AnimationMultiplier => animationMultiplier;

        public bool HasSeed { get; private set; } = false;
        public bool IsMature { get; private set; } = false;

        [SerializeField] float duration = 0.0f;
        [SerializeField] float animationDuration = 0.0f;
        [SerializeField] float animationMultiplier = 1.0f;

        [SerializeField] FarmerProgressBar progressBar = null;
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

			seedObject = Instantiate(GameManager.ItemDataBase.GetSeed(this.seedName + "Step1").prefab);
            seedObject.transform.SetParent(transform, false);

            progressBar.SetActive(true);
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
                progressBar.SetActive(false);
                activableImage.SetActive(true);
            }
            currentGrowingDuration -= Time.deltaTime;

            progressBar.SetPercent(GetPercentGrowing());

            float percentValue = GetTruncatePercentGrowing();
            if (percentValue == 0.5f)
                SetSeedObject(GameManager.ItemDataBase.GetSeed(seedName + "Step2").prefab);
            else if (percentValue == 1.0f)
                SetSeedObject(GameManager.ItemDataBase.GetSeed(seedName + "Step3").prefab);
        }

		public void Select()
		{
            activableImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		}
		public void Deselect()
		{
            activableImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}

		public bool IsInteractable()
		{
            return IsMature;
		}
        public void Interact()
        {
            Player.AddItem(new ItemInfo(plantName, ItemType.Plant, 1));

            IsMature = false;
            activableImage.SetActive(false);

            seedName = "NoSeed";
            plantName = "NoName";
            HasSeed = false;

            if (seedObject != null)
                Destroy(seedObject);
        }

        public void PlayAnim(AnimationController animationController)
		{
            animationController.CharacterPickUp(true, animationController.GetDesiredAnimationSpeed(duration, animationDuration, animationMultiplier));
		}
        public void StopAnim(AnimationController animationController)
		{
            animationController.CharacterPickUp(false);
        }

		private void Update()
		{
            if (!HasSeed) return;

            UpdateGrowingDuration();
		}
    }
}
