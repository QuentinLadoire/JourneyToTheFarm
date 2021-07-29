using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmerCanvas : MonoBehaviour
{
	public new Camera camera = null;

	Canvas canvas = null;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
	}
	private void Start()
	{
		if (camera != null)
			canvas.worldCamera = camera;
	}
	private void Update()
	{
		if (camera != null)
			transform.forward = camera.transform.forward;
	}
}
