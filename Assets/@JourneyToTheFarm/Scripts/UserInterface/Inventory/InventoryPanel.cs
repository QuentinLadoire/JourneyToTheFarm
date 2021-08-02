using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class InventoryPanel : CustomBehaviour
    {
        public Button closeButton = null;
        public Transform[] slotTransforms = null;
        public GameObject itemTemplate = null;

        ItemUI[] itemArray = null;
        PlayerInventoryController controller = null;

        void FillItemArray()
		{
            if (itemArray == null) return;

            var inventory = controller.Inventory;
            for (int i = 0; i < itemArray.Length; i++)
			{
                if (inventory.ItemArray[i] != null)
                {
                    itemArray[i] = Instantiate(itemTemplate).GetComponent<ItemUI>();
                    itemArray[i].SetParent(slotTransforms[i], false);
                    itemArray[i].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
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
            if (controller == null) return;

            ClearItemArray();
            FillItemArray();
        }

        public void SetInventoryController(PlayerInventoryController controller)
		{
            this.controller = controller;

            SetupItemArray();
        }

        void OnClickButton()
        {
            if (controller != null)
                controller.CloseInventory();
        }

        protected override void Awake()
		{
            base.Awake();

            closeButton.onClick.AddListener(OnClickButton);
            itemArray = new ItemUI[slotTransforms.Length];
            itemArray.Fill(null);
		}
	}
}
