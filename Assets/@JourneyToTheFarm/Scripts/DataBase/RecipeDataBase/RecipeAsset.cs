using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;
using JTTF.Inventory;

namespace JTTF.DataBase
{
    [System.Serializable]
    public class Requirement
	{
        public Item item = Item.None;
        public int amount = 0;
	}

    [System.Serializable]
    public class RecipeAsset
    {
        public string name = "NoName";
        public Item item = Item.None;
        public Sprite icon = null;
        public int amount = 0;

        public float craftDuration = 0.0f;
        public Requirement[] requirements = null;
    }
}
