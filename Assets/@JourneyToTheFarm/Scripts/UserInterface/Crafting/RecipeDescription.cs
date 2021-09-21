using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JTTF.Behaviour;

namespace JTTF.UI
{
    public class RecipeDescription : UIBehaviour
    {
        [Header("RecipeDescription Settings")]
        [SerializeField] private Image itemIconImage = null;
        [SerializeField] private Text itemNameText = null;
        [SerializeField] private Text itemDescriptionText = null;
        [SerializeField] private Text craftRequirementText = null;

        public void Init(Sprite itemIcon, string itemName, string itemDescription, string craftRequirement)
		{
            itemIconImage.sprite = itemIcon;
            itemNameText.text = itemName;
            itemDescriptionText.text = itemDescription;
            craftRequirementText.text = craftRequirement;
		}
    }
}
