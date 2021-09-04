using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class FarmPlotProgressBar : UIBehaviour
    {
        [Header("FarmPlotProgressBar")]
        [SerializeField] private Image gaugeImage = null;

        public void SetPercent(float percent)
        {
            gaugeImage.fillAmount = Mathf.Clamp(percent, 0.0f, 1.0f);
        }
    }
}
