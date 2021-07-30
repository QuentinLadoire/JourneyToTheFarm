using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
{
	public class FarmPlotCanvas : MonoBehaviour
	{
		Canvas canvas = null;
		new Camera camera = null;

		private void Awake()
		{
			canvas = GetComponent<Canvas>();
		}
		private void Start()
		{
			camera = GameManager.playerCamera;
			canvas.worldCamera = GameManager.playerCamera;
		}
		private void Update()
		{
			if (camera != null)
				transform.forward = camera.transform.forward;
		}
	}
}
