using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JTTF.Interface;

namespace JTTF.UI
{
    public class ClosableInventoryPanel : InventoryPanel
    {
        [Header("Closable Inventory Panel Parameters")]
        [SerializeField] private Button closeButton = null;

        private IClosable closable = null;

        private void OnClickButton()
        {
            if (closable != null && !closable.Equals(null))
                closable.CloseInventory();
        }

		protected override void Init()
		{
			base.Init();

            closable = controller.GetComponent<IClosable>();
		}

		protected override void Awake()
		{
			base.Awake();

            closeButton.onClick.AddListener(OnClickButton);
        }
	}
}
