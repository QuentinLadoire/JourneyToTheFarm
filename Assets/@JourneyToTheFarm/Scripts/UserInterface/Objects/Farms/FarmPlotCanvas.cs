using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JTTF.Behaviour;
using JTTF.Management;

namespace JTTF.UI
{
	public class FarmPlotCanvas : UIBehaviour
	{
		private Canvas canvas = null;
		private new Camera camera = null;

		protected override void Awake()
		{
			base.Awake();

			canvas = GetComponent<Canvas>();
		}
		protected override void Start()
		{
			base.Start();

			camera = GameManager.cameraController.Camera;
			canvas.worldCamera = GameManager.cameraController.Camera;
		}
		protected override void Update()
		{
			base.Update();

			if (camera != null)
				transform.forward = camera.transform.forward;
		}
	}
}
