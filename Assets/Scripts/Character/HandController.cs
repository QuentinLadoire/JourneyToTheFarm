using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class HandController : MonoBehaviour
	{
		public Action<GameObject> onHandedObjectChange = (GameObject handedObject) => { /*Debug.Log("OnHandedObjectChange");*/ };

		[SerializeField] Transform rightHand = null;
		[SerializeField] Transform leftHand = null;

		GameObject handedObject = null;

		void ClearHandedObject()
		{
			if (handedObject != null)
				Destroy(handedObject);
		}
		void InstantiateObject(ItemType itemType, string name)
		{
			var item = GameManager.ItemDataBase.GetItem(itemType, name);
			if (item != Item.Default && item.prefab != null)
			{
				handedObject = Instantiate(item.prefab);
				onHandedObjectChange.Invoke(handedObject);
			}
		}
		void CheckIsHandable()
		{
			if (handedObject == null) return;

			var handable = handedObject.GetComponent<IHandable>();
			if (handable != null)
				handable.SetHanded(rightHand, leftHand);
		}

		void OnScroll(int index, string name, ItemType itemType)
		{
			ClearHandedObject();

			InstantiateObject(itemType, name);

			CheckIsHandable();
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
