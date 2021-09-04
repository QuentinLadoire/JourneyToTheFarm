using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class ScrollableInventoryPanel : InventoryPanel
    {
        [Header("Scrollable Inventory Panel Parameters")]
        [SerializeField] private Image selectedSlot = null;

        private ShortcutInventoryController sController = null;

        private void SetSelected(int index)
		{
            selectedSlot.transform.position = slotArray[index].transform.position;
		}

        private void OnScroll(int index, Item item)
		{
            SetSelected(index);
		}
        private void OnInventoryChange()
		{
            RefreshInventory();
        }

        protected override void Init()
		{
            base.Init();

            if (controller == null) return;

            sController = controller as ShortcutInventoryController;
            if (sController != null)
            {
                sController.onSelectedSlotChange += OnScroll;
                sController.onInventoryChange += OnInventoryChange;
            }
        }

		protected override void OnDestroy()
		{
            base.OnDestroy();

            if (sController != null)
            {
                sController.onSelectedSlotChange -= OnScroll;
                sController.onInventoryChange -= OnInventoryChange;
            }
		}
	}
}
