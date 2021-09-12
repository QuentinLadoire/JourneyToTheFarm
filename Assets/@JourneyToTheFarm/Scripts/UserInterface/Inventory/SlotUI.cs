using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;
using JTTF.Inventory;

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
		public void SetItem(Item item)
		{
			this.item = item;
			itemUI.SetItem(item);
		}

		public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
		{
			CanvasManager.GamePanel.ParentToDragAndDropPanel(itemUI.transform);
			itemUI.transform.position = eventData.position;
		}
		public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
		{
			itemUI.transform.position = eventData.position;
		}
		public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
		{
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
						if (draggedSlotUI.item.name == item.name)
						{
							var rest = ownerPanel.Controller.StackItemAt(index, draggedSlotUI.item.amount);
							if (rest != -1)
								draggedSlotUI.ownerPanel.Controller.UnstackItemAt(draggedSlotUI.index, draggedSlotUI.item.amount - rest);
						}
						else
							ownerPanel.Controller.SwitchItem(index, draggedSlotUI.ownerPanel.Controller, draggedSlotUI.index);
					}
				}
			}
		}
	}
}
