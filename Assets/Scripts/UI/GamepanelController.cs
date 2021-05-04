using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class GamepanelController : MonoBehaviour
	{
		[SerializeField] InventoryPanel inventoryPanel = null;
		[SerializeField] CraftingPanel craftingPanel = null;
		[SerializeField] CraftingProgressBar craftingProgressBar = null;
		[SerializeField] ChestInventoryPanel chestInventoryPanel = null;

		private void Start()
		{
			inventoryPanel.Init();
			craftingPanel.Init();
			craftingProgressBar.Init();
			chestInventoryPanel.Init();
		}
		private void OnDestroy()
		{
			inventoryPanel.Destroy();
			craftingPanel.Destroy();
			craftingProgressBar.Destroy();
			chestInventoryPanel.Destroy();
		}
	}
}
