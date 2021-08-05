
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class InventoryPanel : UIBehaviour
    {
        [Header("Inventory Panel Parameters")]
        [SerializeField] protected SlotUI[] slotArray = null;
        [SerializeField] protected ItemUI[] itemArray = null;

        protected InventoryController controller = null;

        protected void RefreshInventory()
        {
            if (controller == null) return;

            for (int i = 0; i < itemArray.Length; i++)
                itemArray[i].SetItem(controller.Inventory.ItemArray[i]);
        }

        protected virtual void Init()
        {
            RefreshInventory();
        }

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < slotArray.Length; i++)
                slotArray[i].Init(i, this);
        }

        public void SetInventoryController(InventoryController controller)
        {
            this.controller = controller;

            Init();
        }
    }
}
