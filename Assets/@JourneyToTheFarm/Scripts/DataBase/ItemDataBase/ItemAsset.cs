using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Enum;

namespace JTTF.DataBase
{
	[System.Serializable]
	public class ItemAsset
	{
		public string name = "NoName";
		public ItemType type = ItemType.None;
		public bool stackable = true;
		public int stackCount = 32;
		public Sprite sprite = null;
		public GameObject prefab = null;
		public GameObject collectiblePrefab = null;
	}
}
