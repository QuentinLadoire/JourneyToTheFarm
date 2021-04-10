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

		public void SetSprite(Sprite sprite)
		{
			iconImage.sprite = sprite;
		}
		public void SetSelected(bool value)
		{
			selectedImage.enabled = value;
		}

		private void Awake()
		{
			selectedImage.enabled = false;
		}
	}
}
