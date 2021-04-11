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
		Plant = 3
	}

	[System.Serializable]
	public class Item
	{
		public static Item Default = new Item();

		public string name;
		public ItemType type;
		public bool stackable;
		public Sprite sprite;
		public GameObject prefab;

		public Item(string name = "NoName", ItemType type = ItemType.None, bool stackable = false, Sprite sprite = null, GameObject prefab = null)
		{
			this.name = name;
			this.type = type;
			this.stackable = stackable;
			this.sprite = sprite;
			this.prefab = prefab;
		}
	}
}
