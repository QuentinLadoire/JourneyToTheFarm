using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class InventoryPanel : CustomBehaviour
    {
        [SerializeField] Button closeButton = null;
        [SerializeField] Transform[] slotTransforms = null;
        [SerializeField] GameObject itemTemplate = null;

        ItemUI[] itemArray = null;
        PlayerInventoryController playerController = null;
        ChestInventoryController chestController = null;

        void FillItemArray(Inventory inventory)
		{
            if (itemArray == null) return;

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
        void SetupItemArray(Inventory inventory)
		{
            ClearItemArray();
            FillItemArray(inventory);
        }

        public void SetInventoryController(PlayerInventoryController controller)
		{
            playerController = controller;

            SetupItemArray(playerController.Inventory);
        }
        public void SetInventoryController(ChestInventoryController controller)
		{
            chestController = controller;

            SetupItemArray(chestController.Inventory);
        }

        void OnClickButton()
        {
            if (playerController != null)
                playerController.CloseInventory();

            if (chestController != null)
                chestController.CloseInventory();
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
