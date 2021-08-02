using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Item
    {
        public Item(string name, ItemType type, int amount)
		{
            this.name = name;
            this.type = type;
            this.amount = amount;
		}

        public string name = "NoName";
        public ItemType type = ItemType.None;
        public int amount = 0;

        public bool IsStackable => GameManager.DataBase.GetItemAsset(name, type).stackable;
        public Sprite Sprite => GameManager.DataBase.GetItemAsset(name, type).sprite;
        public GameObject Prefab => GameManager.DataBase.GetItemAsset(name, type).prefab;
    }
}
