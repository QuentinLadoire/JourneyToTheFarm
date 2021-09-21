using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JTTF.Behaviour;

namespace JTTF.UI
{
    public class RecipeButton : UIBehaviour
    {
        [Header("RecipeButton Settings")]
        [SerializeField] private Image itemIconImage = null;
        [SerializeField] private Text itemNameText = null;

		private int index = -1;
        private Button button = null;

		public Action<int> onButtonClicked = (int index) => { /*Debug.Log("OnButtonClicked");*/ };

		private void OnButtonClicked()
		{
			onButtonClicked.Invoke(index);
		}

		protected override void Awake()
		{
			base.Awake();

			button = GetComponent<Button>();
		}
		protected override void Start()
		{
			button.onClick.AddListener(OnButtonClicked);
		}

		public void Init(int index, Sprite itemIcon, string itemName, Action<int> callback)
		{
			this.index = index;
            itemIconImage.sprite = itemIcon;
            itemNameText.text = itemName;
			onButtonClicked += callback;
		}
	}
}
