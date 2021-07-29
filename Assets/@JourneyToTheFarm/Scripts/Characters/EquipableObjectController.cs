using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class EquipableObjectController : MonoBehaviour
	{
		public Action<GameObject> onEquipedObjectChange = (GameObject equipedObject) => { /*Debug.Log("OnEquipedObjectChange");*/ };

		[SerializeField] Transform rightHand = null;
		[SerializeField] Transform leftHand = null;

		int currentIndex = 0;
		GameObject equipedObject = null;

		void ClearEquipedObject()
		{
			if (equipedObject != null)
				Destroy(equipedObject);
		}
		void InstantiateObject(ItemType itemType, string name, int amount)
		{
			if (amount >= 1)
			{
				var item = GameManager.ItemDataBase.GetItem(itemType, name);
				if (item != Item.Default && item.prefab != null)
				{
					equipedObject = Instantiate(item.prefab);
					onEquipedObjectChange.Invoke(equipedObject);
				}
			}
		}
		void CheckIsEquipable()
		{
			if (equipedObject == null) return;

			var handable = equipedObject.GetComponent<IEquipable>();
			if (handable != null)
				handable.Equip(rightHand, leftHand);
		}

		void OnScroll(int index, string name, ItemType itemType, int amount)
		{
			ClearEquipedObject();

			InstantiateObject(itemType, name, amount);

			CheckIsEquipable();

			currentIndex = index;
		}
		void OnAddItem(int index, ItemInfo info)
		{
			if (index == currentIndex)
			{
				ClearEquipedObject();

				InstantiateObject(info.type, info.name, info.amount);

				CheckIsEquipable();
			}
		}
		void OnRemoveItem(int index, ItemInfo info)
		{
			if (currentIndex == index && info.amount == 0)
				ClearEquipedObject();
		}

		private void Awake()
		{
			Player.OnScroll += OnScroll;
			Player.OnAddItem += OnAddItem;
			Player.OnRemoveItem += OnRemoveItem;
		}
		private void OnDestroy()
		{
			Player.OnScroll -= OnScroll;
			Player.OnAddItem -= OnAddItem;
			Player.OnRemoveItem -= OnRemoveItem;
		}
	}
}
