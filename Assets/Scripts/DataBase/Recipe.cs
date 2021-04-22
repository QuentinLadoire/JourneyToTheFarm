using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    [System.Serializable]
    public struct ItemInfo
    {
        public string name;
        public string quantity;
    }

    [System.Serializable]
    public class Recipe
    {
        public string name = "NoName";
        [TextArea] public string description = "NoDescription";
        public ItemInfo[] items = null;
    }
}
