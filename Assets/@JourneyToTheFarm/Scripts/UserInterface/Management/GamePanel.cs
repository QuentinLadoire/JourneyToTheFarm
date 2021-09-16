using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;
using JTTF.Inventory;

#pragma warning disable IDE0044

namespace JTTF.UI
{
    public class GamePanel : UIBehaviour
    {
        [SerializeField] private PlayerPanel playerPanel = null;
        [SerializeField] private InventoryPanel playerInventoryPanel = null;
        [SerializeField] private InventoryPanel chestInventoryPanel = null;
        [SerializeField] private InventoryPanel shortcutInventoryPanel = null;
        [SerializeField] private GameObject dragAndDropPanel = null;

        public PlayerPanel PlayerPanel => playerPanel;

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
