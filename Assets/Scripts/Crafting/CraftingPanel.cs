using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class CraftingPanel : MonoBehaviour
	{
		[SerializeField] GameObject recipeContent = null;
		[SerializeField] GameObject recipePaternPrefab = null;

		[SerializeField] GameObject recipeDescrition = null;

		public void SetActive(bool value)
		{
			gameObject.SetActive(value);
		}
		public void Init()
		{
			foreach (var recipe in Player.CraftingController.RecipeDataBase.Recipes)
			{
				var recipePatern = Instantiate(recipePaternPrefab).GetComponent<RecipePatern>();
				recipePatern.SetParent(recipeContent.transform, false);
				recipePatern.Set(recipe.icon, recipe.name);
			}
		}
		public void Destroy()
		{

		}
	}
}
