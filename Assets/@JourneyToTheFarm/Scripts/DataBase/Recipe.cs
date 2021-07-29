using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    [System.Serializable]
    public class Recipe
    {
        public static Recipe Default = new Recipe();

        public string name = "NoName";
        public ItemType itemType = ItemType.None;
        public Sprite icon = null;
        public int amount = 0;

        [TextArea] public string description = "NoDescription";
        public float craftDuration = 0.0f;
        public ItemInfo[] requirements = null;
    }
}
