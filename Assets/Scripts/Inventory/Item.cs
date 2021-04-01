using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public enum ItemType
    {
        None = -1,
        Item,
        Tool,
        SeedBag,
        Seed,
        Plant
    }

    [System.Serializable]
    public class Item
    {
        public string name = "NoName";
        public ItemType type = ItemType.Item;
        public Sprite sprite = null;
        public GameObject prefab = null;
    }
}
