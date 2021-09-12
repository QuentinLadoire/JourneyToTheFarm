using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF.Behaviour
{
	public class UIBehaviour : CustomBehaviour
	{
		private RectTransform rectTransform = null;

		public RectTransform RectTransform => GetRectTransform();

		private RectTransform GetRectTransform()
		{
			if (rectTransform == null)
				rectTransform = GetComponent<RectTransform>();

			return rectTransform;
		}

		protected override void Awake()
		{
			base.Awake();

			if (rectTransform == null)
				rectTransform = GetComponent<RectTransform>();
		}
	}
}
