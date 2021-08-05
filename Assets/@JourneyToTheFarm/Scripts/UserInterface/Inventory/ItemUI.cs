using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class ItemUI : UIBehaviour, IDragable
	{
		[SerializeField] Image itemIcon = null;
		[SerializeField] Text itemAmount = null;

		Item item = null;
		Transform previousParent = null;
		Vector3 previousPosition = Vector3.zero;

		public Item Item => item;
		public bool Droppped { get; set; } = false;

		public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
		{
			previousParent = transform.parent;
			previousPosition = transform.position;
		}
		public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData eventData)
		{
			CanvasManager.GamePanel.ParentToDragAndDropPanel(transform);
		}
		public void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
		{
			transform.position = eventData.position;
		}
		public void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
		{

		}
		public void OnEndDrag(UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (!Droppped)
			{
				transform.SetParent(previousParent, false);
				transform.position = previousPosition;
			}

			Droppped = false;
		}

		public void SetItem(Item item)
		{
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
		public void Init(Item item)
		{
			this.item = item;

			SetItem(item);
		}
	}
}
