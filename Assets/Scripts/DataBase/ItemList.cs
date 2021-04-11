using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	namespace Deprecated
	{
		public class ItemList : MonoBehaviour
		{
			static ItemList instance = null;

			public static Item GetDefaultItem()
			{
				return instance.defaultItem;
			}
			public static Item GetTool(string name)
			{
				foreach (var tool in instance.tools)
					if (tool.name == name)
						return tool;

				return instance.errorItem;
			}
			public static Item GetSeedBag(string name)
			{
				foreach (var seedBag in instance.seedBags)
					if (seedBag.name == name)
						return seedBag;

				return instance.errorItem;
			}
			public static Item GetSeed(string name)
			{
				foreach (var seed in instance.seeds)
					if (seed.name == name)
						return seed;

				return instance.errorItem;
			}
			public static Item GetPlant(string name)
			{
				foreach (var plant in instance.plants)
					if (name == plant.name)
						return plant;

				return instance.errorItem;
			}

			public static Item GetItem(ItemType itemType, string itemName)
			{
				switch (itemType)
				{
					case ItemType.Tool:
						return GetTool(itemName);

					case ItemType.SeedBag:
						return GetSeedBag(itemName);

					case ItemType.Seed:
						return GetSeed(itemName);

					case ItemType.Plant:
						return GetPlant(itemName);
				}

				return instance.errorItem;
			}

			[SerializeField] Item errorItem = Item.Default;
			[SerializeField] Item defaultItem = Item.Default;
			[SerializeField] Item[] tools = null;
			[SerializeField] Item[] seedBags = null;
			[SerializeField] Item[] seeds = null;
			[SerializeField] Item[] plants = null;

			private void Awake()
			{
				instance = this;
			}
		}
	}
}
