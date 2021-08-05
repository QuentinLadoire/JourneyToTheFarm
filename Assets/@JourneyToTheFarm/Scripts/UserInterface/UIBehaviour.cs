using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : CustomBehaviour
{
	public RectTransform RectTransform => rectTransform;
	RectTransform rectTransform = null;

	protected override void Awake()
	{
		base.Awake();

		rectTransform = GetComponent<RectTransform>();
	}
}
