using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class GamePanel : MonoBehaviour
    {
        public InventoryPanel playerInventoryPanel = null;

        public void OpenPlayerInventory(PlayerInventoryController controller)
		{
            playerInventoryPanel.SetInventoryController(controller);
            playerInventoryPanel.SetActive(true);
		}
        public void ClosePlayerInventory()
		{
            playerInventoryPanel.SetActive(false);
		}
    }
}
