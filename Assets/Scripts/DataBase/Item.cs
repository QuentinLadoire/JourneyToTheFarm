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
		Resource = 4
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
