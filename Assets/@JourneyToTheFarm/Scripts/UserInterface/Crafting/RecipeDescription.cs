using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class RecipeDescription : CustomBehaviour
    {
        [SerializeField] Image itemIcon = null;
        [SerializeField] Text itemName = null;
        [SerializeField] Text itemDescription = null;
        [SerializeField] Text craftRequirement = null;
        [SerializeField] Button craftButton = null;

        public Action onClick = () => { /*Debug.Log("OnClickCraftButton");*/ };

        public void Set(Sprite icon, string name, string description, string requirement)
		{
            itemIcon.sprite = icon;
            itemName.text = name;
            itemDescription.text = description;
            craftRequirement.text = requirement;
		}
        public void SetIcon(Sprite icon)
		{
            itemIcon.sprite = icon;
		}
        public void SetName(string name)
		{
            itemName.text = name;
		}
        public void SetDescription(string description)
        {
            itemDescription.text = description;
        }
        public void SetCraftRequirement(string requirement)
		{
            craftRequirement.text = requirement;
		}

        void OnClick()
		{
            onClick.Invoke();
		}

		protected override void Awake()
		{
            base.Awake();

            craftButton.onClick.AddListener(OnClick);
		}
	}
}
