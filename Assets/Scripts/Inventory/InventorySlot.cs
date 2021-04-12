using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class InventorySlot : MonoBehaviour
    {
		[SerializeField] Image iconImage = null;
		[SerializeField] Image selectedImage = null;

		[SerializeField] Image amountImage = null;
		[SerializeField] Text amountText = null;

		void SetAmountVisible(bool value)
		{
			amountImage.enabled = value;
			amountText.enabled = value;
		}

		public void SetSprite(Sprite sprite)
		{
			iconImage.sprite = sprite;

			SetAmountVisible(sprite != null);
		}
		public void SetAmount(int amount)
		{
			amountText.text = amount.ToString();
		}
		public void SetSelected(bool value)
		{
			selectedImage.enabled = value;
		}

		private void Awake()
		{
			selectedImage.enabled = false;
			SetAmountVisible(false);
		}
	}
}
