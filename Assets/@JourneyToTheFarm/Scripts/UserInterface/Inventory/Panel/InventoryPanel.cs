using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;
using JTTF.Inventory;

namespace JTTF.UI
{
    public class InventoryPanel : UIBehaviour
    {
        [Header("Inventory Panel Parameters")]
        [SerializeField] protected SlotUI[] slotArray = null;

        protected InventoryController controller = null;

        public InventoryController Controller => controller;

        private void OnInventoryChange()
		{
            RefreshInventory();
        }

        protected void RefreshInventory()
        {
            if (controller == null) return;

            for (int i = 0; i < slotArray.Length; i++)
            {
                var index = controller.Inventory.GetIndex(i);
                if (index != -1)
                {
                    slotArray[i].SetItem(controller.Inventory.GetItemAt(index), controller.Inventory.GetAmountAt(index));
                }
                else
				{
                    slotArray[i].SetItem(Item.None, 0);
				}
            }
        }

        protected virtual void Init()
        {
            if (controller == null) return;

            controller.OnInventoryChange += OnInventoryChange;

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
                controller.OnInventoryChange -= OnInventoryChange;
		}

		public void SetInventoryController(InventoryController controller)
        {
            this.controller = controller;

            Init();
        }
    }
}
