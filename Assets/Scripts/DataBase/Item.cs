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
		public string name;
		public ItemType type;
		public int amount;

		public ItemInfo(string name, ItemType type, int amount)
		{
			this.name = name;
			this.type = type;
			this.amount = amount;
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
