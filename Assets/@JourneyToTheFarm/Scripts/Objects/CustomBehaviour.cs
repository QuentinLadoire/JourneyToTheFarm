using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBehaviour : MonoBehaviour
{
	[SerializeField] bool isActiveOnAwake = true;

	protected virtual void Awake()
	{
		SetActive(isActiveOnAwake);
	}
	protected virtual void Start()
	{

	}
	protected virtual void Update()
	{

	}
	protected virtual void OnDestroy()
	{

	}

	public void SetActive(bool value)
	{
		isActiveOnAwake = true;
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
