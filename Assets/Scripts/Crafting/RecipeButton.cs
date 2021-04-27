using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class RecipeButton : MonoBehaviour
    {
        [SerializeField] Image iconRecipe = null;
        [SerializeField] Text nameRecipe = null;

        Button button = null;

        public Action onClick = () => { /*Debug.Log("OnRecipeButtonClick");*/ };

        public void SetParent(Transform parent, bool stayWorldPosition)
		{
            transform.SetParent(parent, stayWorldPosition);
		}

        public void Set(Sprite icon, string name)
		{
            iconRecipe.sprite = icon;
            nameRecipe.text = name;
		}
        public void SetIcon(Sprite icon)
		{
            iconRecipe.sprite = icon;
		}
        public void SetName(string name)
		{
            nameRecipe.text = name;
		}

        void OnClick()
		{
            onClick.Invoke();
        }

		private void Awake()
		{
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }
	}
}
