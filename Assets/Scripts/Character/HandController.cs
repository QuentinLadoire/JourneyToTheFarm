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

		int currentIndex = 0;
		GameObject handedObject = null;

		void ClearHandedObject()
		{
			if (handedObject != null)
				Destroy(handedObject);
		}
		void InstantiateObject(ItemType itemType, string name, int amount)
		{
			if (amount >= 1)
			{
				var item = GameManager.ItemDataBase.GetItem(itemType, name);
				if (item != Item.Default && item.prefab != null)
				{
					handedObject = Instantiate(item.prefab);
					onHandedObjectChange.Invoke(handedObject);
				}
				else
				{
					onHandedObjectChange.Invoke(null);
				}
			}
		}
		void CheckIsHandable()
		{
			if (handedObject == null) return;

			var handable = handedObject.GetComponent<IHandable>();
			if (handable != null)
				handable.SetHanded(rightHand, leftHand);
		}

		void OnScroll(int index, string name, ItemType itemType, int amount)
		{
			ClearHandedObject();

			InstantiateObject(itemType, name, amount);

			CheckIsHandable();

			currentIndex = index;
		}
		void OnRemoveItem(int index, string name, int amount, ItemType itemType)
		{
			if (currentIndex == index && amount == 0)
				ClearHandedObject();
		}

		private void Awake()
		{
			Player.OnScroll += OnScroll;
			Player.OnRemoveItem += OnRemoveItem;
		}
		private void OnDestroy()
		{
			Player.OnScroll -= OnScroll;
			Player.OnRemoveItem -= OnRemoveItem;
		}
	}
}
