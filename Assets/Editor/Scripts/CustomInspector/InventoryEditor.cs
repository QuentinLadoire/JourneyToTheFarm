using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JTTF.Inventory;

namespace JTTFEditor
{
    [CustomEditor(typeof(Inventory))]
    public class InventoryEditor : Editor
    {
		Inventory inventory = null;

		public override void OnInspectorGUI()
		{
			inventory = target as Inventory;

			base.OnInspectorGUI();

			if (!EditorApplication.isPlaying) return;

			GUILayout.Label("Items :");
			for (int i = 0; i < inventory.ItemCount; i++)
			{
				var item = inventory.GetItemAt(i);
				var amount = inventory.GetAmountAt(i);
				GUILayout.Label("  Name : " + item.name + " - Type : " + item.type + " - Amount : " + amount);
			}
		}
	}
}
