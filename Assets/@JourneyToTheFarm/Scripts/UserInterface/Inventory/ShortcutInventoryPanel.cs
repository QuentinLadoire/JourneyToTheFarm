using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class ShortcutInventoryPanel : UIBehaviour
    {
        [SerializeField] Image selectedSlot = null;
        [SerializeField] Transform[] slotTransforms = null;
        [SerializeField] GameObject itemTemplate = null;

        ItemUI[] itemArray = null;
        ShortcutInventoryController controller = null;

        void SetSelected(int index)
		{
            selectedSlot.transform.position = slotTransforms[index].position;
		}

        void FillItemArray()
        {
            if (itemArray == null) return;

            for (int i = 0; i < itemArray.Length; i++)
            {
                var inventory = controller.Inventory;
                if (inventory.ItemArray[i] != null)
                {
                    itemArray[i] = Instantiate(itemTemplate).GetComponent<ItemUI>();
                    itemArray[i].SetParent(slotTransforms[i], false);
                    itemArray[i].RectTransform.anchoredPosition = Vector2.zero;
                    itemArray[i].SetItem(inventory.ItemArray[i]);
                    itemArray[i].SetActive(true);
                }
            }
        }
        void ClearItemArray()
        {
            if (itemArray == null) return;

            for (int i = 0; i < itemArray.Length; i++)
            {
                if (itemArray[i] != null)
                    itemArray[i].Destroy();
            }
        }
        void SetupItemArray()
        {
            ClearItemArray();
            FillItemArray();
        }

        void OnScroll(int index)
		{
            SetSelected(index);
		}
        void OnInventoryChange()
		{
            SetupItemArray();
        }

        void SetupPanel()
		{
            if (controller == null) return;

            controller.onScroll += OnScroll;
            controller.onInventoryChange += OnInventoryChange;

            SetupItemArray();
        }

        public void SetInventoryController(ShortcutInventoryController controller)
		{
            this.controller = controller;
            SetupPanel();
        }

		protected override void Awake()
		{
            base.Awake();

            itemArray = new ItemUI[slotTransforms.Length];
            itemArray.Fill(null);
        }
		private void OnDestroy()
		{
            if (controller != null)
            {
                controller.onScroll -= OnScroll;
                controller.onInventoryChange -= OnInventoryChange;
            }
		}
	}
}
