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

        [SerializeField] RecipeDataBase recipeDataBase = null;

        bool isOpen = false;
        bool inCrafting = false;
        Recipe currentRecipe = null;
        float currentDuration = 0.0f;

        public void StartCraft(Recipe recipe)
		{
            if (recipe == Recipe.Default) return;

            inCrafting = true;
            currentRecipe = recipe;
            currentDuration = recipe.duration;
		}
        public void CancelCraft()
		{
            inCrafting = false;
            currentRecipe = null;
            currentDuration = 0.0f;
		}
        void EndCraft()
		{
            Player.AddItem(currentRecipe.itemType, currentRecipe.name, currentRecipe.amount);

            inCrafting = false;
            currentRecipe = null;
            currentDuration = 0.0f;
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
