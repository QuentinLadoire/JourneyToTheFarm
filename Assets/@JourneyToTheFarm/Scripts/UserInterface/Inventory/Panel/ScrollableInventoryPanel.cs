using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class ScrollableInventoryPanel : InventoryPanel
    {
        [Header("Scrollable Inventory Panel Parameters")]
        [SerializeField] Image selectedSlot = null;

        ShortcutInventoryController sController = null;

        void SetSelected(int index)
		{
            selectedSlot.transform.position = slotArray[index].transform.position;
		}

        void OnScroll(int index, Item item)
		{
            SetSelected(index);
		}
        void OnInventoryChange()
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

		private void OnDestroy()
		{
            if (sController != null)
            {
                sController.onSelectedSlotChange -= OnScroll;
                sController.onInventoryChange -= OnInventoryChange;
            }
		}
	}
}
