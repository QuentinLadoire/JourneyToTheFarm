using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JTTF
{
	public class TransportableController : MonoBehaviour
	{
		public Action<TransportableObject> onTransportableObjectChange = (TransportableObject) => { /*Debug.Log("OnTransportableObjectChange");*/ };

		[SerializeField] Transform rightHandTransform = null;
		[SerializeField] Transform leftHandTransform = null;

		TransportableObject currentObject = null;

		void OnScroll(int index, string itemName, ItemType itemType)
		{
			var toDestroy = currentObject;

			var item = GameManager.ItemDataBase.GetItem(itemType, itemName);
			if (item.prefab != null)
			{
				currentObject = Instantiate(item.prefab).GetComponent<TransportableObject>();
				currentObject.SetHands(rightHandTransform, leftHandTransform);
			}

			onTransportableObjectChange.Invoke(currentObject);

			if (toDestroy != null)
				toDestroy.Destroy();
		}

		private void Awake()
		{
			Player.OnScroll += OnScroll;
		}
		private void OnDestroy()
		{
			Player.OnScroll -= OnScroll;
		}
	}
}
