using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JTTF
{
    public class InventoryPanel : SimpleObject
    {
		public Action<PointerEventData, int, ItemInfo, InventoryController> onBeginDrag = (PointerEventData eventData, int index, ItemInfo info, InventoryController controller) => { /*Debug.Log("OnBeginDrag");*/ };
		public Action<PointerEventData, int, ItemInfo, InventoryController> onDrag = (PointerEventData eventData, int index, ItemInfo info, InventoryController controller) => { /*Debug.Log("OnDrag");*/ };
		public Action<PointerEventData, int, ItemInfo, InventoryController> onEndDrag = (PointerEventData eventData, int index, ItemInfo info, InventoryController controller) => { /*Debug.Log("OnEndDrag");*/ };
		public Action<PointerEventData, int, ItemInfo, InventoryController> onDrop = (PointerEventData eventData, int index, ItemInfo info, InventoryController controller) => { /*Debug.Log("OnDrop");*/ };

		[SerializeField] protected Button closeButton = null;
		[SerializeField] protected InventorySlot[] inventorySlots = null;
		[SerializeField] protected int indexOffset = 0;
		protected InventoryController inventoryController = null;

		bool hasDrag = false;
		bool hasDrop = false;

		protected void OnClick()
		{
			if (inventoryController != null)
				inventoryController.CloseInventory();
		}

		protected void OnSlotBeginDrag(PointerEventData eventData, int index)
		{
			if (!inventoryController.IsEmptyAt(index + indexOffset))
			{
				hasDrag = true;

				inventorySlots[index].SetVisible(false);

				onBeginDrag.Invoke(eventData, index + indexOffset, inventoryController.GetItemInfoAt(index + indexOffset), inventoryController);
			}
		}
		protected void OnSlotDrag(PointerEventData eventData, int index)
		{
			if (hasDrag)
				onDrag.Invoke(eventData, index + indexOffset, inventoryController.GetItemInfoAt(index), inventoryController);
		}
		protected void OnSlotEndDrag(PointerEventData eventData, int index)
		{
			if (hasDrag)
			{
				hasDrag = false;

				if (!hasDrop)
					inventorySlots[index].SetVisible(true);
				else
					hasDrop = false;

				onEndDrag.Invoke(eventData, index + indexOffset, inventoryController.GetItemInfoAt(index), inventoryController);
			}
		}
		protected void OnSlotDrop(PointerEventData eventData, int index)
		{
			hasDrop = true;

			onDrop.Invoke(eventData, index + indexOffset, inventoryController.GetItemInfoAt(index + indexOffset), inventoryController);
		}

		protected void OnAddItem(int index, ItemInfo info)
		{
			if (index - indexOffset < 0) return;

			var item = GameManager.ItemDataBase.GetItem(info.type, info.name);
			inventorySlots[index - indexOffset].SetSprite(item.sprite);
			inventorySlots[index - indexOffset].SetAmount(info.amount);
		}
		protected void OnRemoveItem(int index, ItemInfo info)
		{
			if (index - indexOffset < 0) return;

			var item = GameManager.ItemDataBase.GetItem(info.type, info.name);
			inventorySlots[index - indexOffset].SetSprite(item.sprite);
			inventorySlots[index - indexOffset].SetAmount(info.amount);
		}

		protected virtual void OnInventoryOpen(InventoryController controller)
		{
			SetActive(true);
			inventoryController = controller;
		}
		protected virtual void OnInventoryClose(InventoryController controller)
		{
			SetActive(false);
			inventoryController = null;
		}

		protected void SetupPanel()
		{
			for (int i = 0; i < inventoryController.GetInventorySize(); i++)
			{
				var itemInfo = inventoryController.GetItemInfoAt(i);
				var item = GameManager.ItemDataBase.GetItem(itemInfo.type, itemInfo.name);
				inventorySlots[i].SetSprite(item.sprite);
				inventorySlots[i].SetAmount(itemInfo.amount);
			}
		}
		protected void ClearPanel()
		{
			for (int i = 0; i < inventorySlots.Length; i++)
			{
				inventorySlots[i].SetSprite(null);
				inventorySlots[i].SetAmount(0);
			}
		}

		protected override void Awake()
		{
			base.Awake();

			closeButton.onClick.AddListener(OnClick);

			for (int i = 0; i < inventorySlots.Length; i++)
			{
				inventorySlots[i].SetIndex(i);
				inventorySlots[i].onBeginDrag += OnSlotBeginDrag;
				inventorySlots[i].onDrag += OnSlotDrag;
				inventorySlots[i].onEndDrag += OnSlotEndDrag;
				inventorySlots[i].onDrop += OnSlotDrop;
			}
		}
	}
}
