using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObject : MonoBehaviour
{
	public void Destroy()
	{
		Destroy(gameObject);
	}
}
