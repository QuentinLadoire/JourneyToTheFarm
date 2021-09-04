using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
    public class PlayerInteractionText : UIBehaviour
    {
        [SerializeField] private Text interactionText = null;

        public void SetText(string str)
		{
            interactionText.text = str;
		}
    }
}
