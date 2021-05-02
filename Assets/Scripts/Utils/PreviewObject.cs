using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class PreviewObject : SimpleObject
	{
		[SerializeField] Color blueColor = new Color(230, 255, 255);
		[SerializeField] Color redColor = Color.red;

		MeshRenderer[] meshRenderers = null;

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
		public void SetVisible(bool value)
		{
			foreach (var renderer in meshRenderers)
				renderer.enabled = value;
		}

		private void Awake()
		{
			meshRenderers = GetComponentsInChildren<MeshRenderer>();
		}
	}
}
