using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;
using JTTF.Management;
using MLAPI.Serialization;

#pragma warning disable IDE0090
#pragma warning disable IDE0079
#pragma warning disable CS0659
#pragma warning disable CS0661

namespace JTTF.Inventory
{
	public struct Item : INetworkSerializable
    {
        private static Item noneItem = new Item("NoName", ItemType.None);

        public string name;
        public ItemType type;

        public static Item None => noneItem;
        public bool IsStackable => GameManager.DataBase.GetItemAsset(name, type).stackable;
		public int StackCount => GameManager.DataBase.GetItemAsset(name, type).stackCount;
        public Sprite Sprite => GameManager.DataBase.GetItemAsset(name, type).sprite;
        public GameObject Prefab => GameManager.DataBase.GetItemAsset(name, type).prefab;
		public GameObject CollectiblePrefab => GameManager.DataBase.GetItemAsset(name, type).collectiblePrefab;

		public Item(string name, ItemType type)
		{
			this.name = name;
			this.type = type;
		}

		public void NetworkSerialize(NetworkSerializer serializer)
		{
			serializer.Serialize(ref name);
			serializer.Serialize(ref type);
		}

		public override bool Equals(object obj)
		{
			return obj is Item item &&
				   name == item.name &&
				   type == item.type;
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
