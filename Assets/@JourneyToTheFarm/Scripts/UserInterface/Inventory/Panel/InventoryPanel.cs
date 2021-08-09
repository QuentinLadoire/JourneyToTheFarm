
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

        void OnInventoryChange()
		{
            RefreshInventory();
        }

        protected void RefreshInventory()
        {
            if (controller == null) return;

            for (int i = 0; i < itemArray.Length; i++)
                itemArray[i].SetItem(controller.Inventory.ItemArray[i]);
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
            {
                slotArray[i].Init(i, this);
                itemArray[i].Init(i, this);
            }
        }
		private void OnDestroy()
		{
            if (controller != null)
                controller.onInventoryChange -= OnInventoryChange;
		}

		public void SetInventoryController(InventoryController controller)
        {
            this.controller = controller;

            Init();
        }
        public bool AddItem(Item item)
		{
            if (controller != null)
                return controller.AddItem(item);

            return false;
		}
        public bool RemoveItem(Item item)
		{
            if (controller != null)
                return controller.RemoveItem(item);

            return false;
		}
        public bool AddItemAt(int index, Item item)
		{
            if (controller != null)
                return controller.AddItemAt(index, item);

            return false;
		}
        public bool RemoveItemAt(int index)
		{
            if (controller != null)
                return controller.RemoveItemAt(index);

            return false;
		}
    }
}
