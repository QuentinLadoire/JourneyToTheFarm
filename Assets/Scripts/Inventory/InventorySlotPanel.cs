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

		void OnAddItem(int index, ItemInfo info)
		{
			if (index >= 10) return;

			var item = GameManager.ItemDataBase.GetItem(info.type, info.name);
			inventorySlots[index].SetSprite(item.sprite);
			inventorySlots[index].SetAmount(info.amount);
		}
		void OnRemoveItem(int index, ItemInfo info)
		{
			if (index >= 10) return;

			inventorySlots[index].SetAmount(info.amount);
			if (info.amount == 0)
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
