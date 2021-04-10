using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class ProgressBar : MonoBehaviour
	{
		[SerializeField] Image gaugeImage = null;

		Image backgroundImage = null;

		public void SetVisible(bool value)
		{
			backgroundImage.enabled = value;
			gaugeImage.enabled = value;
		}
		public void SetPercent(float percent)
		{
			gaugeImage.fillAmount = Mathf.Clamp(percent, 0.0f, 1.0f);
		}
		private void Awake()
		{
			backgroundImage = GetComponent<Image>();
		}
	}
}
