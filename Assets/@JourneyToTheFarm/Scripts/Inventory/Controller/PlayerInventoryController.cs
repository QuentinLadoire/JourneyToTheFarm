using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.UI;
using JTTF.Character;
using JTTF.Interface;
using JTTF.Management;

namespace JTTF.Inventory
{
    public class PlayerInventoryController : InventoryController, IOpenable, IClosable
	{
		private bool isOpen = false;
		private Player ownerPlayer = null;

		protected override int InventorySize => 30;

		public Player OwnerPlayer => ownerPlayer;

		protected override void Awake()
		{
			base.Awake();

			ownerPlayer = GetComponent<Player>();
		}
		public override void NetworkStart()
		{
			base.NetworkStart();

			if (!(this.IsClient && this.IsLocalPlayer))
			{
				this.enabled = false;
				return;
			}
		}
		protected override void Update()
		{
			ProcessInput();
		}

		private void ProcessInput()
		{
			if (Input.GetButtonDown("Inventory"))
			{
				if (!isOpen)
					OpenInventory();
				else
					CloseInventory();
			}
		}

		public void OpenInventory()
		{
			isOpen = true;
			CanvasManager.GamePanel.OpenPlayerInventory(this);
			GameManager.ActiveCursor();
			OwnerPlayer.MovementController.DeactiveControl();
			GameManager.cameraController.DeactiveControl();
		}
		public void CloseInventory()
		{
			isOpen = false;
			CanvasManager.GamePanel.ClosePlayerInventory();
			GameManager.DeactiveCursor();
			OwnerPlayer.MovementController.ActiveControl();
			GameManager.cameraController.ActiveControl();
		}
	}
}
