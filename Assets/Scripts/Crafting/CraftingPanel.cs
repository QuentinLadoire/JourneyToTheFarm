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
				str += Player.HowManyItem(requirement.name).ToString() + " / " + requirement.amount.ToString() + " " + requirement.name;
			}
			recipeDescription.SetCraftRequirement(str);

			currentRecipe = recipe;
		}
		void OnClickCraftButton()
		{
			Player.Craft(currentRecipe);
		}

		void OnAddItem(int index, string name, int amount, ItemType itemType)
		{
			string str = "Craft Requirement : ";
			foreach (var requirement in currentRecipe.requirements)
			{
				str += "\n";
				str += Player.HowManyItem(requirement.name).ToString() + " / " + requirement.amount.ToString() + " " + requirement.name;
			}
			recipeDescription.SetCraftRequirement(str);
		}
		void OnRemoveItem(int index, string name, int amoutn, ItemType itemType)
		{
			string str = "Craft Requirement : ";
			foreach (var requirement in currentRecipe.requirements)
			{
				str += "\n";
				str += Player.HowManyItem(requirement.name).ToString() + " / " + requirement.amount.ToString() + " " + requirement.name;
			}
			recipeDescription.SetCraftRequirement(str);
		}

		void OnCraftingOpen()
		{
			gameObject.SetActive(true);

			OnClickRecipeButton(currentRecipe);
		}
		void OnCraftingClose()
		{
			gameObject.SetActive(false);
		}

		public void Init()
		{
			currentRecipe = Player.CraftingRecipe.Recipes[0];
			foreach (var recipe in Player.CraftingRecipe.Recipes)
			{
				var recipePatern = Instantiate(recipePaternPrefab).GetComponent<RecipeButton>();
				recipePatern.SetParent(recipeContent.transform, false);
				recipePatern.Set(recipe.icon, recipe.name);
				recipePatern.onClick += () => OnClickRecipeButton(recipe);
			}

			recipeDescription.onClick += OnClickCraftButton;

			Player.OnCraftingOpen += OnCraftingOpen;
			Player.OnCraftingClose += OnCraftingClose;

			Player.OnAddItem += OnAddItem;
			Player.OnRemoveItem += OnRemoveItem;
		}
		public void Destroy()
		{
			Player.OnCraftingOpen -= OnCraftingOpen;
			Player.OnCraftingClose -= OnCraftingClose;

			Player.OnAddItem -= OnAddItem;
			Player.OnRemoveItem -= OnRemoveItem;
		}
	}
}
