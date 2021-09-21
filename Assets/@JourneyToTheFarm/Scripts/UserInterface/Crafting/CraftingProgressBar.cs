using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JTTF.Behaviour;

namespace JTTF.UI
{
    public class CraftingProgressBar : UIBehaviour
    {
        [Header("CraftingProgressBar Settings")]
        [SerializeField] private Image gaugeImage = null;
        [SerializeField] private Text recipeText = null;

        public void Init(string recipeName)
		{
            recipeText.text = recipeName;
            gaugeImage.fillAmount = 0.0f;
		}
        public void SetFillAmount(float percent)
		{
            gaugeImage.fillAmount = percent;
		}
    }
}
