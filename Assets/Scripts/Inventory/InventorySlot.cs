using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class InventorySlot : SimpleObject
    {
		[SerializeField] Image iconImage = null;
		[SerializeField] Text amountText = null;

		public void SetSprite(Sprite sprite)
		{
			iconImage.sprite = sprite;
		}
		public void SetAmount(int amount)
		{
			amountText.text = amount.ToString();
		}
	}
}
