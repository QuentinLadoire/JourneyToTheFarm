using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    [CreateAssetMenu(fileName = "NewRecipeDataBase", menuName = "Create New RecipeDataBase")]
    public class RecipeDataBase : ScriptableObject
    {
        public List<Recipe> Recipes = new List<Recipe>();

        public Recipe GetRecipe(string name)
		{
            foreach (var recipe in Recipes)
                if (recipe.name == name)
                    return recipe;

            return Recipe.Default;
		}
    }
}
