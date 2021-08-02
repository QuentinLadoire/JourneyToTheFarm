using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class ItemUI : CustomBehaviour
    {
        public Image itemIcon = null;
        public Text itemAmount = null;

        public void SetItem(Item item)
		{
            itemIcon.sprite = item.Sprite;
            itemAmount.text = item.amount.ToString();
		}
    }
}
