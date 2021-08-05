using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class ClosableInventoryPanel : InventoryPanel
    {
        [Header("Closable Inventory Panel Parameters")]
        [SerializeField] Button closeButton = null;

        IClosable closable = null;

        void OnClickButton()
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
