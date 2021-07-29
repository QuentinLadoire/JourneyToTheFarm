using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JTTF
{
	public class DragAndDropPanel : CustomBehaviour
	{
		[SerializeField] Image draggedIconImage = null;
		[SerializeField] Image draggedAmountImage = null;
		[SerializeField] Text draggedAmountText = null;

		void SetVisible(bool value)
		{
			SetIconVisible(value);
			SetAmountVisible(value);
		}
		void SetIconVisible(bool value)
		{
			draggedIconImage.enabled = value;
		}
		void SetAmountVisible(bool value)
		{
			draggedAmountImage.enabled = value;
			draggedAmountText.enabled = value;
		}

		void OnBeginDrag(PointerEventData eventData, ItemInfo info)
		{
			if (info.type != ItemType.None)
			{
				var item = GameManager.ItemDataBase.GetItem(info.type, info.name);
				draggedIconImage.sprite = item.sprite;
				draggedIconImage.transform.position = eventData.position;

				SetIconVisible(true);
				if (info.amount > 1)
				{
					draggedAmountText.text = info.amount.ToString();
					SetAmountVisible(true);
				}
			}
		}
		void OnDrag(PointerEventData eventData, ItemInfo info)
		{
			draggedIconImage.transform.position = eventData.position;
		}
		void OnEndDrag(PointerEventData eventData, ItemInfo info)
		{
			SetVisible(false);
		}

		protected override void Awake()
		{
			base.Awake();

			SetVisible(false);

			InventoryPanel.onBeginDrag += OnBeginDrag;
			InventoryPanel.onDrag += OnDrag;
			InventoryPanel.onEndDrag += OnEndDrag;
		}
		private void OnDestroy()
		{
			InventoryPanel.onBeginDrag -= OnBeginDrag;
			InventoryPanel.onDrag -= OnDrag;
			InventoryPanel.onEndDrag -= OnEndDrag;
		}
	}
}
