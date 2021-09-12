using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.Behaviour;

namespace JTTF
{
	public class PreviewObject : CustomBehaviour
	{
		[SerializeField] private Color blueColor = new Color(230, 255, 255);
		[SerializeField] private Color redColor = Color.red;

		private MeshRenderer[] meshRenderers = null;

		protected override void Awake()
		{
			base.Awake();

			meshRenderers = GetComponentsInChildren<MeshRenderer>();
		}

		public void SetBlueColor()
		{
			foreach (var renderer in meshRenderers)
				renderer.material.color = blueColor;
		}
		public void SetRedColor()
		{
			foreach (var renderer in meshRenderers)
				renderer.material.color = redColor;
		}
	}
}
