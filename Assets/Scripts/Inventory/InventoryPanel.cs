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
		public static Action<PointerEventData, ItemInfo> onBeginDrag = (PointerEventData eventData, ItemInfo info) => { /*Debug.Log("OnBeginDrag");*/ };
		public static Action<PointerEventData, ItemInfo> onDrag = (PointerEventData eventData, ItemInfo info) => { /*Debug.Log("OnDrag");*/ };
		public static Action<PointerEventData, ItemInfo> onEndDrag = (PointerEventData eventData, ItemInfo info) => { /*Debug.Log("OnEndDrag");*/ };

		static DraggedItemInfo draggedItemInfo = DraggedItemInfo.Default;

		[SerializeField] Button closeButton = null;
		[SerializeField] InventorySlot[] inventorySlots = null;
		[SerializeField] int indexOffset = 0;
		protected InventoryController inventoryController = null;

		void OnSlotBeginDrag(PointerEventData eventData, int index, ItemInfo info)
		{
			inventoryController.DragItemAt(index + indexOffset);

			onBeginDrag.Invoke(eventData, info);
		}
		void OnSlotDrag(PointerEventData eventData, int index, ItemInfo info)
		{
			onDrag.Invoke(eventData, info);
		}
		void OnSlotEndDrag(PointerEventData eventData, int index, ItemInfo info)
		{
			inventoryController.CancelDrag();

			onEndDrag.Invoke(eventData, info);
		}
		void OnSlotDrop(PointerEventData eventData, int index, ItemInfo info)
		{
			inventoryController.DropItemAt(index + indexOffset);
		}

		protected virtual void OnInventoryOpen(InventoryController controller)
		{
			SetActive(true);

			inventoryController = controller;
			inventoryController.onAddItem += OnAddItem;
			inventoryController.onRemoveItem += OnRemoveItem;

			SetupPanel();
		}
		protected virtual void OnInventoryClose(InventoryController controller)
		{
			ClearPanel();
			
			inventoryController.onAddItem -= OnAddItem;
			inventoryController.onRemoveItem -= OnRemoveItem;
			inventoryController = null;

			SetActive(false);
		}

		void OnAddItem(int index, ItemInfo info)
		{
			if (index - indexOffset < 0) return;

			inventorySlots[index - indexOffset].SetItem(info);
		}
		void OnRemoveItem(int index, ItemInfo info)
		{
			if (index - indexOffset < 0) return;

			inventorySlots[index - indexOffset].SetItem(info);
		}

		void SetupPanel()
		{
			for (int i = 0; i < inventorySlots.Length; i++)
			{
				var itemInfo = inventoryController.GetItemInfoAt(i + indexOffset);
				inventorySlots[i].SetItem(itemInfo);
			}
		}
		void ClearPanel()
		{
			for (int i = 0; i < inventorySlots.Length; i++)
				inventorySlots[i].SetItem(ItemInfo.Default);
		}

		void OnClick()
		{
			if (inventoryController != null)
				inventoryController.CloseInventory();
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
