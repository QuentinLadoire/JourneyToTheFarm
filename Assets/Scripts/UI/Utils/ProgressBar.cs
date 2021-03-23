using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class ProgressBar : MonoBehaviour
	{
		Image image = null;

		public void SetVisible(bool value)
		{
			image.enabled = value;
		}
		public void SetPercent(float percent)
		{
			image.fillAmount = Mathf.Clamp(percent, 0.0f, 1.0f);
		}

		private void Awake()
		{
			image = GetComponent<Image>();
		}
	}
}
