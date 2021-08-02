using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public enum ItemType
	{
		None = -1,
		Tool,
		Seed,
		Resource,
		Container,
		Workbench
	}

	[System.Serializable]
	public class ItemAsset
	{
		public string name = "NoName";
		public ItemType type = ItemType.None;
		public bool stackable = true;
		public Sprite sprite = null;
		public GameObject prefab = null;
	}
}
