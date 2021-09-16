using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;
using JTTF.Inventory;

#pragma warning disable IDE0044

namespace JTTF.UI
{
	public class SlotUI : UIBehaviour, IDragable, IDropable
	{
		[SerializeField] private ItemUI itemUI = null;

		private int index = -1;
		private Item item = Item.None;
		private InventoryPanel ownerPanel = null;

		public void Init(int index, InventoryPanel owner)
		{
			this.index = index;
			ownerPanel = owner;
		}
		public void SetItem(Item item, int amount)
		{
			this.item = item;
			itemUI.SetItem(item, amount);
		}

		public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (item == Item.None) return;

			CanvasManager.GamePanel.ParentToDragAndDropPanel(itemUI.transform);
			itemUI.transform.position = eventData.position;
		}
		public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (item == Item.None) return;

			itemUI.transform.position = eventData.position;
		}
		public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (item == Item.None) return;

			itemUI.transform.SetParent(transform, false);
			itemUI.RectTransform.anchoredPosition = Vector2.zero;
		}
		public void OnDrop(UnityEngine.EventSystems.PointerEventData eventData)
		{
			var draggedObject = eventData.pointerDrag;
			if (draggedObject != null)
			{
				var draggedSlotUI = draggedObject.GetComponent<SlotUI>();
				if (draggedSlotUI != null)
				{
					if (draggedSlotUI != this) //if not him self
					{
						if (item == Item.None) //if we need to move or swap item
						{
							if (draggedSlotUI.ownerPanel == ownerPanel) //Move Item on same Panel
							{
								draggedSlotUI.ownerPanel.Controller.MoveItem(draggedSlotUI.index, index);
							}
							else //Move Item on another Panel
							{
								draggedSlotUI.ownerPanel.Controller.MoveItem(draggedSlotUI.index, ownerPanel.Controller.Inventory, index);
							}
						}
						else
						{
							if (draggedSlotUI.ownerPanel == ownerPanel) //Swap Item on same Panel
							{
								draggedSlotUI.ownerPanel.Controller.SwapItem(draggedSlotUI.index, index);
							}
							else //Swap Item on another Panel
							{
								draggedSlotUI.ownerPanel.Controller.SwapItem(draggedSlotUI.index, ownerPanel.Controller.Inventory, index);
							}
						}
					}
				}
			}
		}
	}
}
