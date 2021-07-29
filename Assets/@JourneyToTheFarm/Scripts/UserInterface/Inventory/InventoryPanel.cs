using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JTTF
{
	public class InventoryPanel : CustomBehaviour
    {
		public static Action<PointerEventData, ItemInfo> onBeginDrag = (PointerEventData eventData, ItemInfo info) => { /*Debug.Log("OnBeginDrag");*/ };
		public static Action<PointerEventData, ItemInfo> onDrag = (PointerEventData eventData, ItemInfo info) => { /*Debug.Log("OnDrag");*/ };
		public static Action<PointerEventData, ItemInfo> onEndDrag = (PointerEventData eventData, ItemInfo info) => { /*Debug.Log("OnEndDrag");*/ };

		[SerializeField] Button closeButton = null;
		[SerializeField] protected InventorySlot[] inventorySlots = null;
		[SerializeField] int indexOffset = 0;
		protected InventoryController inventoryController = null;

		bool hasDrag = false;

		void OnSlotPointerDown(PointerEventData eventData, int index)
		{
			inventoryController.DragItemAt(index + indexOffset);

			onBeginDrag.Invoke(eventData, inventoryController.GetDraggedItem().info);
		}
		void OnSlotBeginDrag(PointerEventData eventData, int index)
		{
			hasDrag = true;
		}
		void OnSlotDrag(PointerEventData eventData, int index)
		{
			onDrag.Invoke(eventData, inventoryController.GetDraggedItem().info);
		}
		void OnSlotEndDrag(PointerEventData eventData, int index)
		{
			hasDrag = false;

			inventoryController.CancelDrag();

			onEndDrag.Invoke(eventData, inventoryController.GetDraggedItem().info);
		}
		void OnSlotPointerUp(PointerEventData eventData, int index)
		{
			if (!hasDrag)
			{
				inventoryController.CancelDrag();

				onEndDrag.Invoke(eventData, inventoryController.GetDraggedItem().info);
			}
		}
		void OnSlotDrop(PointerEventData eventData, int index)
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

		protected void OnAddItem(int index, ItemInfo info)
		{
			if (index - indexOffset < 0 || index - indexOffset >= inventorySlots.Length) return;

			inventorySlots[index - indexOffset].SetItem(info);
		}
		protected void OnRemoveItem(int index, ItemInfo info)
		{
			if (index - indexOffset < 0 || index - indexOffset >= inventorySlots.Length) return;

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

			if (closeButton != null)
				closeButton.onClick.AddListener(OnClick);

			for (int i = 0; i < inventorySlots.Length; i++)
			{
				inventorySlots[i].SetIndex(i);

				inventorySlots[i].onPointerDown += OnSlotPointerDown;
				inventorySlots[i].onBeginDrag += OnSlotBeginDrag;
				inventorySlots[i].onDrag += OnSlotDrag;
				inventorySlots[i].onEndDrag += OnSlotEndDrag;
				inventorySlots[i].onPointerUp += OnSlotPointerUp;

				inventorySlots[i].onDrop += OnSlotDrop;
			}
		}
	}
}
