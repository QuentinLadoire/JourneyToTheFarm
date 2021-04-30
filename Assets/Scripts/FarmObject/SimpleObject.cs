using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObject : MonoBehaviour
{
	public void SetActive(bool value)
	{
		gameObject.SetActive(value);
	}
	public void Destroy()
	{
		Destroy(gameObject);
	}
}
