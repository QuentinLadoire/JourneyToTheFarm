using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class ItemUI : UIBehaviour
	{
		[SerializeField] private Image itemIcon = null;
		[SerializeField] private Text itemAmount = null;

		public void SetItem(Item item)
		{
			itemIcon.sprite = item.Sprite;
			itemAmount.text = item.amount.ToString();

			SetActive(item != Item.None);
		}
	}
}
