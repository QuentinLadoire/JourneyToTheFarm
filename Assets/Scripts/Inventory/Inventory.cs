using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public struct ItemContainer
	{
		public Item Item { get; set; }
		public int Amount { get; set; }

		public ItemContainer(Item item = null, int amount = 0)
		{
			Item = item;
			Amount = amount;
		}
	}

	public class Inventory : MonoBehaviour
	{
		public Action<int, ItemContainer> onScroll = (int index, ItemContainer itemContainer) => { /*Debug.Log("OnScrollUp");*/ };
		public Action<int, ItemContainer> onAddItemShortcut = (int index, ItemContainer itemContainer) => { /*Debug.Log("OnNewItemShortcut index : " + index);*/ };

		ItemContainer[] slotShortcut = new ItemContainer[10];
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
		void ScrollInput()
		{
			float delta = Input.GetAxis("ScrollShortcut");

			if (delta > 0.0f)
				ScrollUp();
			else if (delta < 0.0f)
				ScrollDown();
		}

		public void AddItemAtShortcut(int index, Item item, int amount)
		{
			if (index < 0 || index > 9) { Debug.Log("Index is out of range"); return; }

			slotShortcut[index].Item = item;
			slotShortcut[index].Amount = amount;

			onAddItemShortcut.Invoke(index, slotShortcut[index]);
		}
		public void AddItem(Item item, int amount = 1)
		{
			for (int i = 0; i < slotShortcut.Length; i++)
				if (slotShortcut[i].Item == null)
				{
					AddItemAtShortcut(i, item, amount);
					return;
				}

			Debug.Log("Can't add item, no empty slot");
		}

		private void Start()
		{
			onScroll.Invoke(shortcutIndex, slotShortcut[shortcutIndex]);
		}
		private void Update()
		{
			ScrollInput();
		}
	}
}
