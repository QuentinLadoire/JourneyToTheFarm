using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ChestInventoryController : MonoBehaviour
    {
        public Inventory Inventory => inventory;

		public Action onInventotyClose = () => { /*Debug.Log("OnInventoryClose");*/ };

        readonly Inventory inventory = new Inventory(30);

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
