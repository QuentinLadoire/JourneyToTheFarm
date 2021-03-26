using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public enum ItemType
    {
        None = -1,
        Item,
        Tool
    }

    [System.Serializable]
    public class Item
    {
        public string name = "NoName";
        public ItemType type = ItemType.Item;
        public Sprite sprite = null;
    }

    [System.Serializable]
    public class ToolItem : Item
    {
        public GameObject prefab = null;

        public ToolItem()
        {
            type = ItemType.Tool;
        }
    }
}
