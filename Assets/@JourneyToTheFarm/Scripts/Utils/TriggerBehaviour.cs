using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerBehaviour : MonoBehaviour
{
	public Action<Collider> onTriggerEnter = (Collider) => { };
	public Action<Collider> onTriggerStay = (Collider) => { };
	public Action<Collider> onTriggerExit = (Collider) => { };

	private void OnTriggerEnter(Collider other)
	{
		onTriggerEnter.Invoke(other);	
	}
	private void OnTriggerStay(Collider other)
	{
		onTriggerStay.Invoke(other);
	}
	private void OnTriggerExit(Collider other)
	{
		onTriggerExit.Invoke(other);
	}
}
