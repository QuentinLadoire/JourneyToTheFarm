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

		[SerializeField] Item errorItem = null;
		[SerializeField] Item defaultItem = null;
		[SerializeField] ToolItem[] tools = null;

		private void Awake()
		{
			instance = this;
		}
	}
}
