using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBehaviour : MonoBehaviour
{
	public bool isActiveOnAwake = true;

	protected virtual void Awake()
	{
		SetActive(isActiveOnAwake);
	}

	public void SetActive(bool value)
	{
		gameObject.SetActive(value);
	}
	public void SetParent(Transform parent, bool worldPositionStay)
	{
		transform.SetParent(parent, worldPositionStay);
	}
	public void Destroy()
	{
		Destroy(gameObject);
	}
}