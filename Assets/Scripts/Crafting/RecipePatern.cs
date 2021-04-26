using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class RecipePatern : MonoBehaviour
    {
        [SerializeField] Image iconRecipe = null;
        [SerializeField] Text nameRecipe = null;

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
    }
}
