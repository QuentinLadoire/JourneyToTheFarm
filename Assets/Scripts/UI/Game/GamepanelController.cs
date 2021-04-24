using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class GamepanelController : MonoBehaviour
	{
		[SerializeField] InventoryPanel inventoryPanel = null;
		[SerializeField] CraftingPanel craftingPanel = null;

		void OnInventoryOpen()
		{
			if (inventoryPanel != null)
				inventoryPanel.SetActive(true);
		}
		void OnInventoryClose()
		{
			if (inventoryPanel != null)
				inventoryPanel.SetActive(false);
		}

		void OnCraftingOpen()
		{
			if (craftingPanel != null)
				craftingPanel.SetActive(true);
		}
		void OnCraftingClose()
		{
			if (craftingPanel != null)
				craftingPanel.SetActive(false);
		}

		private void Start()
		{
			Player.OnInventoryOpen += OnInventoryOpen;
			Player.OnInventoryClose += OnInventoryClose;

			Player.OnCraftingOpen += OnCraftingOpen;
			Player.OnCraftingClose += OnCraftingClose;

			inventoryPanel.Init();
			craftingPanel.Init();
		}
		private void OnDestroy()
		{
			Player.OnInventoryOpen -= OnInventoryOpen;
			Player.OnInventoryClose -= OnInventoryClose;

			Player.OnCraftingOpen -= OnCraftingOpen;
			Player.OnCraftingClose -= OnCraftingClose;

			inventoryPanel.Destroy();
			craftingPanel.Destroy();
		}
	}
}
