using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class PlayerPanel : UIBehaviour
	{
		[SerializeField] PlayerProgressBar playerProgressBar = null;

		public PlayerProgressBar PlayerProgressBar => playerProgressBar;
	}
}