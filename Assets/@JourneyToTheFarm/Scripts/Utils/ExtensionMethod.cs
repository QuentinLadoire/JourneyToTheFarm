using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
	public static void Fill<T>(this T[] array, T value)
	{
		for (int i = 0; i < array.Length; i++)
			array[i] = value;
	}

    public static Vector3 RoundToInt(this Vector3 vector)
	{
		return new Vector3(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
	}
}
