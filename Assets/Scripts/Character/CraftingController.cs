using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class CraftingController : MonoBehaviour
    {
        public RecipeDataBase RecipeDataBase { get => recipeDataBase; }

        public Action onOpen = () => { /*Debug.Log("OnOpen");*/ };
        public Action onClose = () => { /*Debug.Log("OnClose");*/ };

        public Action<Recipe> onStartCraft = (Recipe) => { /*Debug.Log("OnStartCraft");*/ };
        public Action onCancelCraft = () => { /*Debug.Log("OnCancelCraft");*/ };
        public Action onEndCraft = () => { /*Debug.Log("OnEndCraft");*/ };
        public Action<float> onCraft = (float percent) => { /*Debug.Log("OnCraft");*/ };

        [SerializeField] RecipeDataBase recipeDataBase = null;

        bool isOpen = false;
        bool inCrafting = false;
        Recipe currentRecipe = null;
        float currentDuration = 0.0f;

        public void StartCraft(Recipe recipe)
		{
            if (inCrafting) CancelCraft();
            if (recipe == Recipe.Default) return;

            if (!CanCraft(recipe)) return;

            inCrafting = true;
            currentRecipe = recipe;
            currentDuration = recipe.craftDuration;

            onStartCraft.Invoke(recipe);
        }
        public void CancelCraft()
		{
            inCrafting = false;
            currentRecipe = null;
            currentDuration = 0.0f;

            onCancelCraft.Invoke();
		}
        void EndCraft()
		{
            Player.AddItem(currentRecipe.name, currentRecipe.amount, currentRecipe.itemType);
            foreach (var requirement in currentRecipe.requirements)
                Player.RemoveItem(requirement.name, requirement.amount);

            inCrafting = false;
            currentRecipe = null;
            currentDuration = 0.0f;

            onEndCraft.Invoke();
		}

        bool CanCraft(Recipe recipe)
		{
            foreach (var requirement in recipe.requirements)
                if (!Player.HasItem(requirement.name, requirement.amount))
                    return false; 

            return true;
		}
        float GetPercentDuration()
		{
            return 1 - (currentDuration / currentRecipe.craftDuration);
		}

        void OpeningInput()
		{
            if (Input.GetButtonDown("Crafting"))
			{
                isOpen = !isOpen;
                if (isOpen)
                    onOpen.Invoke();
                else
                    onClose.Invoke();
			}
		}
        void UpdateDuration()
		{
            if (!inCrafting) return;

            onCraft.Invoke(GetPercentDuration());

            if (currentDuration <= 0.0f)
                EndCraft();

            currentDuration -= Time.deltaTime;
		}

        private void Update()
		{
            OpeningInput();
            UpdateDuration();
        }
	}
}
