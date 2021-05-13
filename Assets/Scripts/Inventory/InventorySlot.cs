using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JTTF
{
    public class InventorySlot : SimpleObject, IDragable, IDropable
    {
		public Action<PointerEventData, int, ItemInfo> onBeginDrag = (PointerEventData eventData, int index, ItemInfo info) => { /*Debug.Log("OnBeginDrag");*/ };
		public Action<PointerEventData, int, ItemInfo> onDrag = (PointerEventData eventData, int index, ItemInfo info) => { /*Debug.Log("OnDrag");*/ };
		public Action<PointerEventData, int, ItemInfo> onEndDrag = (PointerEventData eventData, int index, ItemInfo info) => { /*Debug.Log("OnEndDrag");*/ };
		public Action<PointerEventData, int, ItemInfo> onDrop = (PointerEventData eventData, int index, ItemInfo info) => { /*Debug.Log("OnDrop");*/ };

		[SerializeField] Image iconImage = null;
		[SerializeField] Image amountImage = null;
		[SerializeField] Text amountText = null;

		public ItemInfo itemInfo = ItemInfo.Default;
		public int index = -1;

		public void SetVisible(bool value)
		{
			iconImage.enabled = value;
			amountImage.enabled = value;
			amountText.enabled = value;
		}
		public void SetIconVisible(bool value)
		{
			iconImage.enabled = value;
		}
		public void SetAmountVisible(bool value)
		{
			amountImage.enabled = value;
			amountText.enabled = value;
		}

		public void SetItem(ItemInfo info)
		{
			itemInfo = info;

			if (itemInfo.type != ItemType.None)
			{
				var item = GameManager.ItemDataBase.GetItem(info.type, info.name);
				SetSprite(item.sprite);
				SetAmount(info.amount);
			}
			else
			{
				SetSprite(null);
				SetAmount(0);
			}
		}
		public void SetIndex(int index)
		{
			this.index = index;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			onBeginDrag.Invoke(eventData, index, itemInfo);
		}
		public void OnDrag(PointerEventData eventData)
		{
			onDrag.Invoke(eventData, index, itemInfo);
		}
		public void OnEndDrag(PointerEventData eventData)
		{
			onEndDrag.Invoke(eventData, index, itemInfo);
		}
		public void OnDrop(PointerEventData eventData)
		{
			onDrop.Invoke(eventData, index, itemInfo);
		}

		public void SetSprite(Sprite sprite)
		{
			iconImage.sprite = sprite;

			if (iconImage == null)
				SetVisible(false);
			else
				SetVisible(true);
		}
		public void SetAmount(int amount)
		{
			amountText.text = amount.ToString();

			if (amount <= 1)
				SetAmountVisible(false);
			else
				SetAmountVisible(true);
		}

		protected override void Awake()
		{
			base.Awake();

			SetVisible(false);
			SetAmount(0);
		}
	}
}
