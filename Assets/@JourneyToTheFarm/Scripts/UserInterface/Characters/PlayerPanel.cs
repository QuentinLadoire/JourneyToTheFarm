using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class PlayerPanel : UIBehaviour
	{
		[SerializeField] private PlayerProgressBar playerProgressBar = null;
		[SerializeField] private PlayerInteractionText playerInteractionText = null;

		public PlayerProgressBar PlayerProgressBar => playerProgressBar;
		public PlayerInteractionText PlayerInteractionText => playerInteractionText;
	}
}