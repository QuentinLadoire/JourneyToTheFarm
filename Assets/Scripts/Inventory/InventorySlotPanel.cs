using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class InventorySlotPanel : SimpleObject
	{
		[SerializeField] Image selectedImage = null;
		[SerializeField] InventorySlot[] inventorySlots = null;

		void OnAddItem(int index, string name, int amount, ItemType itemType)
		{
			if (index >= 10) return;

			var item = GameManager.ItemDataBase.GetItem(itemType, name);
			inventorySlots[index].SetSprite(item.sprite);
			inventorySlots[index].SetAmount(amount);
		}
		void OnRemoveItem(int index, string name, int amount, ItemType itemType)
		{
			if (index >= 10) return;

			inventorySlots[index].SetAmount(amount);
			if (amount == 0)
				inventorySlots[index].SetSprite(null);
		}
		void OnScroll(int index, string name, ItemType itemType, int amount)
		{
			SelectSlotAt(index);
		}

		void SelectSlotAt(int index)
		{
			selectedImage.transform.position = inventorySlots[index].transform.position;
		}

		protected override void Awake()
		{
			base.Awake();

			Player.OnAddItem += OnAddItem;
			Player.OnRemoveItem += OnRemoveItem;
			Player.OnScroll += OnScroll;
		}
		private void OnDestroy()
		{
			Player.OnAddItem -= OnAddItem;
			Player.OnRemoveItem -= OnRemoveItem;
			Player.OnScroll -= OnScroll;
		}
	}
}
