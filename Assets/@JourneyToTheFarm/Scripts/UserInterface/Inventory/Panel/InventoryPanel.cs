
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class InventoryPanel : UIBehaviour
    {
        [Header("Inventory Panel Parameters")]
        [SerializeField] protected SlotUI[] slotArray = null;

        protected InventoryController controller = null;

        public InventoryController Controller => controller;

        void OnInventoryChange()
		{
            RefreshInventory();
        }

        protected void RefreshInventory()
        {
            if (controller == null) return;

            for (int i = 0; i < slotArray.Length; i++)
                slotArray[i].SetItem(controller.Inventory.ItemArray[i]);
        }

        protected virtual void Init()
        {
            if (controller == null) return;

            controller.onInventoryChange += OnInventoryChange;

            RefreshInventory();
        }

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < slotArray.Length; i++)
                slotArray[i].Init(i, this);
        }
		protected override void OnDestroy()
		{
            base.OnDestroy();

            if (controller != null)
                controller.onInventoryChange -= OnInventoryChange;
		}

		public void SetInventoryController(InventoryController controller)
        {
            this.controller = controller;

            Init();
        }
    }
}
