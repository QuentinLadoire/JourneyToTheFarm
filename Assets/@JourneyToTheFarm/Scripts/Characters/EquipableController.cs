using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class EquipableController : MonoBehaviour
    {
        [SerializeField] Transform rightHand = null;
        [SerializeField] Transform leftHand = null;

        GameObject equipedObject = null;

        public Player OwnerPlayer { get; private set; } = null;

        public Action<GameObject> onEquipedObjectChange = (GameObject) => { /*Debug.Log("OnEquipedObjectChange");*/ };

        private void Awake()
        {
            OwnerPlayer = GetComponent<Player>();
        }
		private void Start()
		{
			OwnerPlayer.ShortcutController.onScroll += OnScroll;
		}
		private void OnDestroy()
		{
			OwnerPlayer.ShortcutController.onScroll -= OnScroll;
		}

		void ClearEquipedObject()
		{
			if (equipedObject != null)
				Destroy(equipedObject);
		}
		void InstantiateObject(Item item)
		{
			if (item != null)
			{
				equipedObject = Instantiate(item.Prefab);

				onEquipedObjectChange.Invoke(equipedObject);
			}
		}
		void CheckIsEquipable()
		{
			if (equipedObject == null) return;

			var equipable = equipedObject.GetComponent<IEquipable>();
			if (equipable != null)
				equipable.Equip(rightHand, leftHand);
		}
		void CheckIsOwnable()
		{
			if (equipedObject == null) return;

			var ownable = equipedObject.GetComponent<IOwnable>();
			if (ownable != null)
				ownable.SetOwner(OwnerPlayer);
		}

		void ChangeEquipedObject(Item item)
		{
			ClearEquipedObject();
			InstantiateObject(item);
			CheckIsOwnable();
			CheckIsEquipable();
		}

		void OnScroll(int index, Item item)
		{
			ChangeEquipedObject(item);
		}
	}
}
