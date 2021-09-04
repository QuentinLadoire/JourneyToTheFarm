using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class CanvasManager : MonoBehaviour
    {
        private static CanvasManager instance = null;

		public static GamePanel GamePanel => instance.gamePanel;

		[SerializeField] private GamePanel gamePanel = null;

		private void Awake()
		{
			instance = this;
		}
	}
}
