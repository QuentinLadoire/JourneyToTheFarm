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
	[System.Serializable]
	public struct Item : INetworkSerializable
    {
        private static Item noneItem = new Item("NoName", ItemType.None);

        public string name;
        public ItemType type;

        public static Item None => noneItem;
        public bool IsStackable => GameManager.ItemDataBase.GetItemAsset(name, type).stackable;
		public int StackCount => GameManager.ItemDataBase.GetItemAsset(name, type).stackCount;
        public Sprite Sprite => GameManager.ItemDataBase.GetItemAsset(name, type).sprite;
        public GameObject Prefab => GameManager.ItemDataBase.GetItemAsset(name, type).prefab;
		public GameObject CollectiblePrefab => GameManager.ItemDataBase.GetItemAsset(name, type).collectiblePrefab;

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
