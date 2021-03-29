using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
	[SerializeField] Material blueMat = null;
	[SerializeField] Material redMat = null;

	MeshRenderer[] meshRenderers = null;

	public void SetBlueMat()
	{
		foreach (var renderer in meshRenderers)
			renderer.material.color = blueMat.color;
	}
	public void SetRedMat()
	{
		foreach (var renderer in meshRenderers)
			renderer.material.color = redMat.color;
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
