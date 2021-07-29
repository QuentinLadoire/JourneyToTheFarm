using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	[CreateAssetMenu(fileName = "NewItemDataBase", menuName = "Create New ItemDataBase")]
	public class ItemDataBase : ScriptableObject
	{
		public List<Item> tools = new List<Item>();
		public List<Item> seedBags = new List<Item>();
		public List<Item> seeds = new List<Item>();
		public List<Item> plants = new List<Item>();
		public List<Item> resources = new List<Item>();
		public List<Item> containers = new List<Item>();
		public List<Item> craftings = new List<Item>();

		public Item GetTool(string name)
		{
			foreach (var tool in tools)
				if (tool.name == name)
					return tool;

			return Item.Default;
		}
		public Item GetSeedBag(string name)
		{
			foreach (var seedBag in seedBags)
				if (seedBag.name == name)
					return seedBag;

			return Item.Default;
		}
		public Item GetSeed(string name)
		{
			foreach (var seed in seeds)
				if (seed.name == name)
					return seed;

			return Item.Default;
		}
		public Item GetPlant(string name)
		{
			foreach (var plant in plants)
				if (plant.name == name)
					return plant;

			return Item.Default;
		}
		public Item GetResource(string name)
		{
			foreach (var resources in resources)
				if (resources.name == name)
					return resources;

			return Item.Default;
		}
		public Item GetContainer(string name)
		{
			foreach (var container in containers)
				if (container.name == name)
					return container;

			return Item.Default;
		}
		public Item GetCrafting(string name)
		{
			foreach (var crafting in craftings)
				if (crafting.name == name)
					return crafting;

			return Item.Default;
		}

		public Item GetItem(ItemType type, string name)
		{
			return type switch
			{
				ItemType.Tool => GetTool(name),
				ItemType.SeedBag => GetSeedBag(name),
				ItemType.Seed => GetSeed(name),
				ItemType.Plant => GetPlant(name),
				ItemType.Resource => GetResource(name),
				ItemType.Container => GetContainer(name),
				ItemType.Crafting => GetCrafting(name),
				_ => Item.Default
			};
		}
	}
}