using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransportableObject : SimpleObject, ITransportable
{
	public Action<Transform, Transform> onSetHands = (Transform rightHand, Transform leftHand) => { /*Debug.Log("OnSetHand");*/ };

	public void SetHands(Transform rightHand, Transform leftHand)
	{
		onSetHands.Invoke(rightHand, leftHand);
	}
}
