using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    [System.Serializable]
    public struct ItemInfo
    {
        public string name;
        public int amount;
    }

    [System.Serializable]
    public class Recipe
    {
        public static Recipe Default = new Recipe();

        public string name = "NoName";
        public ItemType itemType = ItemType.None;
        public int amount = 0;

        [TextArea] public string description = "NoDescription";
        public float duration = 0.0f;
        public ItemInfo[] items = null;
    }
}
