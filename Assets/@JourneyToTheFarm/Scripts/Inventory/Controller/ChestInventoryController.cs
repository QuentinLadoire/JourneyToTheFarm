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
			GameManager.ActiveCursor();
			GameManager.player.CharacterController.DesactiveControl();
		}
		public void CloseInventory()
		{
			CanvasManager.GamePanel.CloseChestInventory();
			GameManager.DesactiveCursor();
			GameManager.player.CharacterController.ActiveControl();

			onInventotyClose.Invoke();
		}
	}
}
