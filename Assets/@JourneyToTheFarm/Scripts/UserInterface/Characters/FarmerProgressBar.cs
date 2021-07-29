using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class FarmerProgressBar : CustomBehaviour
	{
		[Header("FarmerProgressBar")]
		public Image backgroundImage = null;
		public Image gaugeImage = null;

		public void SetPercent(float percent)
		{
			gaugeImage.fillAmount = Mathf.Clamp(percent, 0.0f, 1.0f);
		}

		protected override void Awake()
		{
			backgroundImage = GetComponent<Image>();
		}
	}
}
