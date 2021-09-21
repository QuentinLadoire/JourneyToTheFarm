using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.UI;
using JTTF.Interface;
using JTTF.Management;

namespace JTTF.Inventory
{
    public class ChestInventoryController : InventoryController, IOpenable, IClosable
    {
		protected override int InventorySize => 30;

		public Action onInventotyClose = () => { /*Debug.Log("OnInventoryClose");*/ };

		public void OpenInventory()
		{
			CanvasManager.GamePanel.OpenChestInventory(this);
			GameManager.player.MovementController.DeactiveControl();
			GameManager.cameraController.DeactiveControl();
			GameManager.ActiveCursor();
		}
		public void CloseInventory()
		{
			CanvasManager.GamePanel.CloseChestInventory();
			GameManager.player.MovementController.ActiveControl();
			GameManager.cameraController.ActiveControl();
			GameManager.DeactiveCursor();

			onInventotyClose.Invoke();
		}
	}
}
