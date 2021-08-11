using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class PlayerPanel : UIBehaviour
	{
		[SerializeField] PlayerProgressBar playerProgressBar = null;
		[SerializeField] PlayerInteractionText playerInteractionText = null;

		public PlayerProgressBar PlayerProgressBar => playerProgressBar;
		public PlayerInteractionText PlayerInteractionText => playerInteractionText;
	}
}