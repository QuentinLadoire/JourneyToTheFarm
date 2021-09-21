using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Inventory;

namespace JTTF.DataBase
{
    [CreateAssetMenu(fileName = "NewRecipeDataBase", menuName = "DataBase/RecipeDataBase")]
    public class RecipeDataBase : ScriptableObject
    {
        public List<RecipeAsset> playerRecipes = new List<RecipeAsset>();

        public RecipeAsset GetPlayerRecipeAsset(Item item)
        {
            foreach (var recipe in playerRecipes)
                if (recipe.item == item)
                    return recipe;

            return null;
        }
    }
}
