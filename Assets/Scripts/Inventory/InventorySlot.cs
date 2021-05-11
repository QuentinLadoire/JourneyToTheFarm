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
		public Action<PointerEventData, int> onBeginDrag = (PointerEventData eventData, int index) => { /*Debug.Log("OnBeginDrag");*/ };
		public Action<PointerEventData, int> onDrag = (PointerEventData eventData, int index) => { /*Debug.Log("OnDrag");*/ };
		public Action<PointerEventData, int> onEndDrag = (PointerEventData eventData, int index) => { /*Debug.Log("OnEndDrag");*/ };
		public Action<PointerEventData, int> onDrop = (PointerEventData eventData, int index) => { /*Debug.Log("OnDrop");*/ };

		[SerializeField] Image iconImage = null;
		[SerializeField] Image amountImage = null;
		[SerializeField] Text amountText = null;

		int index = -1;

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

		public void SetSlot(int index, Sprite sprite, int amount)
		{
			this.index = index;
			iconImage.sprite = sprite;
			amountText.text = amount.ToString();
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
		public void SetIndex(int index)
		{
			this.index = index;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			onBeginDrag.Invoke(eventData, index);
		}
		public void OnDrag(PointerEventData eventData)
		{
			onDrag.Invoke(eventData, index);
		}
		public void OnEndDrag(PointerEventData eventData)
		{
			onEndDrag.Invoke(eventData, index);
		}
		public void OnDrop(PointerEventData eventData)
		{
			onDrop.Invoke(eventData, index);
		}

		protected override void Awake()
		{
			base.Awake();

			SetVisible(false);
			SetAmount(0);
		}
	}
}
