using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupBehaviour : UIBehaviour
{
    private CanvasGroup canvasGroup = null;

	public CanvasGroup CanvasGroup => canvasGroup;

	protected override void Awake()
	{
		base.Awake();

		canvasGroup = GetComponent<CanvasGroup>();
	}
}
