using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class CraftingPanel : MonoBehaviour
	{
		[SerializeField] GameObject recipeContent = null;
		[SerializeField] GameObject recipePaternPrefab = null;

		[SerializeField] RecipeDescription recipeDescription = null;

		Recipe currentRecipe = null;

		void OnClickRecipeButton(Recipe recipe)
		{
			recipeDescription.SetIcon(recipe.icon);
			recipeDescription.SetName(recipe.name);
			recipeDescription.SetDescription(recipe.description);

			string str = "Craft Requirement : ";
			foreach (var requirement in recipe.requirements)
			{
				str += "\n";
				str += Player.HasItem(requirement.name).ToString() + " / " + requirement.amount.ToString() + " " + requirement.name;
			}
			recipeDescription.SetCraftRequirement(str);

			currentRecipe = recipe;
		}
		void OnClickCraftButton()
		{
			Debug.Log("Craft " + currentRecipe.name);
		}

		void OnCraftingOpen()
		{
			OnClickRecipeButton(currentRecipe);
		}

		public void SetActive(bool value)
		{
			gameObject.SetActive(value);
		}
		public void Init()
		{
			currentRecipe = Player.CraftingController.RecipeDataBase.Recipes[0];
			foreach (var recipe in Player.CraftingController.RecipeDataBase.Recipes)
			{
				var recipePatern = Instantiate(recipePaternPrefab).GetComponent<RecipeButton>();
				recipePatern.SetParent(recipeContent.transform, false);
				recipePatern.Set(recipe.icon, recipe.name);
				recipePatern.onClick += () => OnClickRecipeButton(recipe);
			}

			recipeDescription.onClick += OnClickCraftButton;

			Player.OnCraftingOpen += OnCraftingOpen;
		}
		public void Destroy()
		{
			Player.OnCraftingOpen -= OnCraftingOpen;
		}
	}
}
