using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : CustomBehaviour
{
	public RectTransform RectTransform => GetRectTransform();
	RectTransform rectTransform = null;

	RectTransform GetRectTransform()
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
