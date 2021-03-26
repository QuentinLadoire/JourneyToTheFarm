using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class InventorySlot : MonoBehaviour
    {
		[SerializeField] Image selectedImage = null;

        Image itemImage = null;

		public void SetSprite(Sprite sprite)
		{
			itemImage.sprite = sprite;
		}
		public void SetSelected(bool value)
		{
			selectedImage.enabled = value;
		}

		private void Awake()
		{
			itemImage = GetComponent<Image>();
			
			selectedImage.enabled = false;
		}
	}
}
