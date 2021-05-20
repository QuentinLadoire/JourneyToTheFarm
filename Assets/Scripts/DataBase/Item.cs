using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public enum ItemType
	{
		None = -1,
		Tool = 0,
		SeedBag = 1,
		Seed = 2,
		Plant = 3,
		Resource = 4,
		Container = 5,
		Crafting = 6
	}

	[System.Serializable]
	public struct ItemInfo
	{
		public static ItemInfo Default => new ItemInfo("None", ItemType.None, 0);

		public string name;
		public ItemType type;
		public int amount;

		public ItemInfo(string name, ItemType type, int amount)
		{
			this.name = name;
			this.type = type;
			this.amount = amount;
		}

		public static bool operator ==(ItemInfo info1, ItemInfo info2)
		{
			return info1.name == info2.name && info1.type == info2.type && info1.amount == info2.amount;
		}
		public static bool operator !=(ItemInfo info1, ItemInfo info2)
		{
			return info1.name != info2.name || info1.type != info2.type || info1.amount != info2.amount;
		}

		public override bool Equals(object obj)
		{
			return obj is ItemInfo info &&
				   name == info.name &&
				   type == info.type &&
				   amount == info.amount;
		}
		public override int GetHashCode()
		{
			int hashCode = 1851593700;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
			hashCode = hashCode * -1521134295 + type.GetHashCode();
			hashCode = hashCode * -1521134295 + amount.GetHashCode();
			return hashCode;
		}
	}

	[System.Serializable]
	public class Item
	{
		public static Item Default = new Item();

		public string name = "NoName";
		public ItemType type = ItemType.None;
		public bool stackable = false;
		public Sprite sprite = null;
		public GameObject prefab = null;
	}
}
