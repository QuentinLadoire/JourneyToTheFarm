using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JTTF.Behaviour;

namespace JTTF.UI
{
	public class PlayerProgressBar : UIBehaviour
	{
		[Header("FarmerProgressBar")]
		[SerializeField] private Image gaugeImage = null;

		public void SetPercent(float percent)
		{
			gaugeImage.fillAmount = Mathf.Clamp(percent, 0.0f, 1.0f);
		}
	}
}
