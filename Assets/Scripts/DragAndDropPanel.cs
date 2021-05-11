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
		[SerializeField] Image draggedIconImage = null;
		[SerializeField] Image draggedAmountImage = null;
		[SerializeField] Text draggedAmountText = null;

		int draggedIndex = -1;
		ItemInfo draggedItemInfo;
		InventoryController draggedController = null;

		void OnBeginDrag(PointerEventData eventData, int index, ItemInfo info, InventoryController controller)
		{
			draggedIndex = index;
			draggedItemInfo = info;
			draggedController = controller;

			var item = GameManager.ItemDataBase.GetItem(info.type, info.name);

			draggedIconImage.enabled = true;
			draggedIconImage.sprite = item.sprite;

			if (info.amount > 1)
			{
				draggedAmountImage.enabled = true;
				draggedAmountText.enabled = true;
				draggedAmountText.text = info.amount.ToString();
			}

			draggedIconImage.transform.position = eventData.position;
		}
		void OnDrag(PointerEventData eventData, int index, ItemInfo info, InventoryController controller)
		{
			draggedIconImage.rectTransform.position = eventData.position;
		}
		void OnEndDrag(PointerEventData eventData, int index, ItemInfo info, InventoryController controller)
		{
			draggedIndex = -1;
			draggedController = null;

			draggedIconImage.enabled = false;
			draggedIconImage.sprite = null;

			draggedAmountImage.enabled = false;
			draggedAmountText.enabled = false;
			draggedAmountText.text = "";
		}
		void OnDrop(PointerEventData eventData, int index, ItemInfo info, InventoryController controller)
		{
			if (!(index == draggedIndex && controller == draggedController))
			{
				controller.AddItemAt(index, draggedItemInfo);
				draggedController.RemoveItemAt(draggedIndex);
			}
		}

		protected override void Awake()
		{
			base.Awake();

			draggedIconImage.enabled = false;
			draggedAmountImage.enabled = false;
			draggedAmountText.enabled = false;

			playerInventoryPanel.onBeginDrag += OnBeginDrag;
			playerInventoryPanel.onDrag += OnDrag;
			playerInventoryPanel.onEndDrag += OnEndDrag;
			playerInventoryPanel.onDrop += OnDrop;

			chestInventoryPanel.onBeginDrag += OnBeginDrag;
			chestInventoryPanel.onDrag += OnDrag;
			chestInventoryPanel.onEndDrag += OnEndDrag;
			chestInventoryPanel.onDrop += OnDrop;
		}
		private void OnDestroy()
		{
			playerInventoryPanel.onBeginDrag -= OnBeginDrag;
			playerInventoryPanel.onDrag -= OnDrag;
			playerInventoryPanel.onEndDrag -= OnEndDrag;
			playerInventoryPanel.onDrop -= OnDrop;

			chestInventoryPanel.onBeginDrag -= OnBeginDrag;
			chestInventoryPanel.onDrag -= OnDrag;
			chestInventoryPanel.onEndDrag -= OnEndDrag;
			chestInventoryPanel.onDrop -= OnDrop;
		}
	}
}
