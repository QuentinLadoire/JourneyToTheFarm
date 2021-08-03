using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class ItemUI : CustomBehaviour
    {
        [SerializeField] Image itemIcon = null;
        [SerializeField] Text itemAmount = null;

        public void SetItem(Item item)
		{
            itemIcon.sprite = item.Sprite;
            itemAmount.text = item.amount.ToString();
		}
    }
}
