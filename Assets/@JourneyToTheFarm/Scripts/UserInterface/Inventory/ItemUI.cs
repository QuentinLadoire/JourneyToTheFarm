using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class ItemUI : UIBehaviour, IDragable, IDropable
	{
		[SerializeField] Image itemIcon = null;
		[SerializeField] Text itemAmount = null;

		int index = -1;
		Item item = null;
		InventoryPanel ownerPanel = null;
		Transform previousParent = null;
		Vector2 previousPosition = Vector2.zero;

		public Item Item => item;
		public int Index => index;
		public InventoryPanel OwnerPanel => ownerPanel;

		protected override void Awake()
		{
			base.Awake();

			previousParent = transform.parent;
			previousPosition = RectTransform.anchoredPosition;
		}

		public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
		{
			CanvasManager.GamePanel.ParentToDragAndDropPanel(transform);
			transform.position = eventData.position;
		}
		public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
		{
			
		}
		public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
		{
			transform.position = eventData.position;
		}
		public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
		{
			transform.SetParent(previousParent, false);
			RectTransform.anchoredPosition = previousPosition;
		}
		public void OnDrop(UnityEngine.EventSystems.PointerEventData eventData)
		{
			var draggedObject = eventData.pointerDrag;
			if (draggedObject != null)
			{
				var itemUI = draggedObject.GetComponent<ItemUI>();
				if (itemUI != null)
				{
					if (itemUI.item.name != item.name) //switch item when they are not identical 
					{
						var itemTmp = new Item(item.name, item.type, item.amount);

						ownerPanel.AddItemAt(index, itemUI.item);
						itemUI.RemoveSelfItemFromInventory();

						itemUI.OwnerPanel.AddItemAt(itemUI.Index, itemTmp);
					}
					else //stack item when they are identical
					{
						ownerPanel.AddItem(itemUI.item);
						itemUI.RemoveSelfItemFromInventory();
					}
				}
			}
		}
		public void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
		{
			
		}

		public void SetItem(Item item)
		{
			this.item = item;

			if (item == null)
			{
				itemIcon.sprite = Item.Default.Sprite;
				itemAmount.text = Item.Default.amount.ToString();

				SetActive(false);
			}
			else
			{
				itemIcon.sprite = item.Sprite;
				itemAmount.text = item.amount.ToString();

				SetActive(true);
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
