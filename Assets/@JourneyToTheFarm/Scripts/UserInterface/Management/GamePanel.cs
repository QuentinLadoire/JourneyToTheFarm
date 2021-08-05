using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class GamePanel : UIBehaviour
    {
        [SerializeField] InventoryPanel playerInventoryPanel = null;
        [SerializeField] InventoryPanel chestInventoryPanel = null;
        [SerializeField] InventoryPanel shortcutInventoryPanel = null;
        [SerializeField] GameObject dragAndDropPanel = null;

        public void OpenPlayerInventory(PlayerInventoryController controller)
		{
            playerInventoryPanel.SetInventoryController(controller);
            playerInventoryPanel.SetActive(true);
		}
        public void ClosePlayerInventory()
		{
            playerInventoryPanel.SetActive(false);
		}

        public void OpenChestInventory(ChestInventoryController controller)
		{
            chestInventoryPanel.SetInventoryController(controller);
            chestInventoryPanel.SetActive(true);
		}
        public void CloseChestInventory()
		{
            chestInventoryPanel.SetActive(false);
        }

        public void InitShortcutInventory(ShortcutInventoryController controller)
		{
            shortcutInventoryPanel.SetInventoryController(controller);
		}

        public void ParentToDragAndDropPanel(Transform transform)
		{
            if (transform != null)
                transform.SetParent(dragAndDropPanel.transform, true);
		}
    }
}