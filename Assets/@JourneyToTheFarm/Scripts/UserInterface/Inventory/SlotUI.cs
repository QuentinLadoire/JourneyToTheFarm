using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class SlotUI : UIBehaviour, IDropable
	{
		int index = -1;
		InventoryPanel ownerPanel = null;

		public void OnDrop(UnityEngine.EventSystems.PointerEventData eventData)
		{
			var draggedObject = eventData.pointerDrag;
			if (draggedObject != null)
			{
				var itemUI = draggedObject.GetComponent<ItemUI>();
				if (itemUI != null)
				{
					if (!(itemUI.OwnerPanel == ownerPanel && itemUI.Index == index)) //move item
					{
						ownerPanel.AddItemAt(index, itemUI.Item);
						itemUI.RemoveSelfItemFromInventory();
					}
				}
			}
		}

		public void Init(int index, InventoryPanel owner)
		{
			this.index = index;
			ownerPanel = owner;
		}

		public bool RemoveSelfItemFromInventory()
		{
			if (ownerPanel != null)
				return ownerPanel.RemoveItemAt(index);

			return false;
		}
	}
}
