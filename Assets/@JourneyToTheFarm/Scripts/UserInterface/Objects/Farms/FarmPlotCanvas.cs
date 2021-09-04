using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF
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

			camera = GameManager.playerCamera;
			canvas.worldCamera = GameManager.playerCamera;
		}
		protected override void Update()
		{
			base.Update();

			if (camera != null)
				transform.forward = camera.transform.forward;
		}
	}
}
