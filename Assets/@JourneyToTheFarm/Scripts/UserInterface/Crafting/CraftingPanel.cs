using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JTTF.Behaviour;
using JTTF.Character;
using JTTF.Management;

namespace JTTF.UI
{
    public class CraftingPanel : UIBehaviour
    {
        [Header("CraftingPanel Settings")]
        [SerializeField] private RecipeButton recipeButtonTemplate = null;
        [SerializeField] private RecipeDescription recipeDescription = null;

        private int selectedRecipeIndex = 0;
        private CraftingController controller = null;

        private void OnRecipeButtonClicked(int index)
		{
            selectedRecipeIndex = index;
            InitRecipeDescription();
		}

        private void InitRecipeButton()
		{
            int index = 0;
            foreach (var recipe in GameManager.RecipeDataBase.playerRecipes)
			{
                var newRecipeButton = Instantiate(recipeButtonTemplate).GetComponent<RecipeButton>();
                newRecipeButton.SetParent(recipeButtonTemplate.transform.parent, false);
                newRecipeButton.Init(index, recipe.icon, recipe.name, OnRecipeButtonClicked);
                newRecipeButton.SetActive(true);
                index++;
			}
		}

        public void InitRecipeDescription()
		{
            if (GameManager.RecipeDataBase.playerRecipes.Count <= 0) return;
            var recipe = GameManager.RecipeDataBase.playerRecipes[selectedRecipeIndex];

            var requirementString = "Craft requirement : \n";
            foreach (var requirement in recipe.requirements)
            {
                var amount = controller.OwnerPlayer.GetAmountOf(requirement.item);
                requirementString += "- " + amount + "/" + requirement.amount + " " + requirement.item.name + "\n";
            }

            recipeDescription.Init(recipe.icon, recipe.name, "NoDescription", requirementString);
		}
        public void Init(CraftingController controller)
		{
            this.controller = controller;

            InitRecipeButton();
            InitRecipeDescription();
        }

        public void OnCraftButtonClicked()
		{
            if (GameManager.RecipeDataBase.playerRecipes.Count <= 0) return;

            var recipe = GameManager.RecipeDataBase.playerRecipes[selectedRecipeIndex];
            controller.StartCraft(recipe);
		}
    }
}
