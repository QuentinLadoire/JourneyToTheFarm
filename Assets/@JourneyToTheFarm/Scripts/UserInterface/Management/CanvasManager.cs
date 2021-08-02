using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class CanvasManager : MonoBehaviour
    {
        static CanvasManager instance = null;

		public static GamePanel GamePanel => instance.gamePanel;

		public GamePanel gamePanel = null;

		private void Awake()
		{
			instance = this;
		}
	}
}
