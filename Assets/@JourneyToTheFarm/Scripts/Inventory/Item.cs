using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0659
#pragma warning disable 0661

namespace JTTF
{
    public struct Item
    {
        static Item noneItem = new Item("NoName", ItemType.None, 0);

        public string name;
        public ItemType type;
        public int amount;

        public static Item None => noneItem;
        public bool IsStackable => GameManager.DataBase.GetItemAsset(name, type).stackable;
		public int StackCount => GameManager.DataBase.GetItemAsset(name, type).stackCount;
        public Sprite Sprite => GameManager.DataBase.GetItemAsset(name, type).sprite;
        public GameObject Prefab => GameManager.DataBase.GetItemAsset(name, type).prefab;
		public GameObject CollectiblePrefab => GameManager.DataBase.GetItemAsset(name, type).collectiblePrefab;

		public Item(string name, ItemType type, int amount)
		{
			this.name = name;
			this.type = type;
			this.amount = amount;
		}

		public override bool Equals(object obj)
		{
			return obj is Item item &&
				   name == item.name &&
				   type == item.type &&
				   amount == item.amount;
		}

		public static bool operator ==(Item left, Item right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(Item left, Item right)
		{
			return !(left == right);
		}
	}
}
