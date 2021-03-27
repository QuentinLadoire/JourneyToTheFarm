using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
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
		public static Item GetSeed(string name)
		{
			foreach (var seed in instance.seeds)
				if (seed.name == name)
					return seed;

			return instance.errorItem;
		}

		[SerializeField] Item errorItem = null;
		[SerializeField] Item defaultItem = null;
		[SerializeField] Item[] tools = null;
		[SerializeField] Item[] seeds = null;

		private void Awake()
		{
			instance = this;
		}
	}
}
