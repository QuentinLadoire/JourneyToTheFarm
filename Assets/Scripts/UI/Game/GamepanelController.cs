using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class GamepanelController : MonoBehaviour
	{
		[SerializeField] GameObject inventoryPanel = null;

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

		private void Start()
		{
			Player.OnInventoryOpen += OnInventoryOpen;
			Player.OnInventoryClose += OnInventoryClose;
		}
		private void OnDestroy()
		{
			Player.OnInventoryOpen -= OnInventoryOpen;
			Player.OnInventoryClose -= OnInventoryClose;
		}
	}
}
