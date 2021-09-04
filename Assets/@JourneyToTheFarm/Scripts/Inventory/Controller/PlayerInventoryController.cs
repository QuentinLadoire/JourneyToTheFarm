using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
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
		protected override void Start()
		{
			base.Start();

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

		void ProcessInput()
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
			OwnerPlayer.CharacterController.DesactiveControl();
		}
		public void CloseInventory()
		{
			isOpen = false;
			CanvasManager.GamePanel.ClosePlayerInventory();
			GameManager.DesactiveCursor();
			OwnerPlayer.CharacterController.ActiveControl();
		}
	}
}
