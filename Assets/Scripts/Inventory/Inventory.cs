using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Inventory : MonoBehaviour
	{
		public Action<int, Item> onScroll = (int index, Item item) => { /*Debug.Log("OnScrollUp");*/ };
		public Action<int, Item> onAddItemShortcut = (int index, Item item) => { /*Debug.Log("OnNewItemShortcut index : " + index);*/ };

		Item[] slotShortcut = new Item[10];
		int shortcutIndex = 0;

		void ScrollUp()
		{
			shortcutIndex--;
			if (shortcutIndex == -1)
				shortcutIndex = 9;

			onScroll.Invoke(shortcutIndex, slotShortcut[shortcutIndex]);
		}
		void ScrollDown()
		{
			shortcutIndex++;
			if (shortcutIndex == 10)
				shortcutIndex = 0;

			onScroll.Invoke(shortcutIndex, slotShortcut[shortcutIndex]);
		}
		void ScrollShortcut()
		{
			float delta = Input.GetAxis("ScrollShortcut");

			if (delta > 0.0f)
				ScrollUp();
			else if (delta < 0.0f)
				ScrollDown();
		}

		public void AddItemAtShortcut(int index, Item item)
		{
			if (index < 0 || index > 9) { Debug.Log("Index is out of range"); return; }

			slotShortcut[index] = item;

			onAddItemShortcut.Invoke(index, item);
		}

		private void Start()
		{
			AddItemAtShortcut(0, ItemList.GetTool("Shovel"));
			AddItemAtShortcut(1, ItemList.GetSeed("WheatSeed"));
			onScroll.Invoke(shortcutIndex, slotShortcut[shortcutIndex]);
		}
		private void Update()
		{
			ScrollShortcut();
		}
	}
}
