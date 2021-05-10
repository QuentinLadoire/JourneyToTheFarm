using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JTTF
{
	public class DragAndDropPanel : SimpleObject
	{
		[SerializeField] InventoryPanel playerInventoryPanel = null;
		[SerializeField] InventoryPanel chestInventoryPanel = null;
		[SerializeField] Image draggedImage = null;

		void OnBeginDrag(PointerEventData eventData, int index, ItemInfo info)
		{
			var item = GameManager.ItemDataBase.GetItem(info.type, info.name);

			draggedImage.enabled = true;
			draggedImage.sprite = item.sprite;
			draggedImage.transform.position = eventData.position;
		}
		void OnDrag(PointerEventData eventData, int index, ItemInfo info)
		{
			draggedImage.rectTransform.position = eventData.position;
		}
		void OnEndDrag(PointerEventData eventData, int index, ItemInfo info)
		{
			draggedImage.enabled = false;
			draggedImage.sprite = null;
		}

		protected override void Awake()
		{
			base.Awake();

			draggedImage.enabled = false;

			playerInventoryPanel.onBeginDrag += OnBeginDrag;
			playerInventoryPanel.onDrag += OnDrag;
			playerInventoryPanel.onEndDrag += OnEndDrag;
		}
		private void OnDestroy()
		{
			playerInventoryPanel.onBeginDrag -= OnBeginDrag;
			playerInventoryPanel.onDrag -= OnDrag;
			playerInventoryPanel.onEndDrag -= OnEndDrag;
		}
	}
}
