using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmPlotPreview : MonoBehaviour
{
	[SerializeField] Material blueMat = null;
	[SerializeField] Material redMat = null;

    MeshRenderer[] meshRenderers = null;

	public void SetBlueMat()
	{
		foreach (var renderer in meshRenderers)
			renderer.materials[1].color = blueMat.color;
	}
	public void SetRedMat()
	{
		foreach (var renderer in meshRenderers)
			renderer.materials[1].color = redMat.color;
	}

	private void Awake()
	{
		meshRenderers = GetComponentsInChildren<MeshRenderer>();
	}
}
