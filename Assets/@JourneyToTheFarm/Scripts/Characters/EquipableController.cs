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

        protected GameObject equipedObject = null;

		public GameObject EquipedObject => equipedObject;
        public Player OwnerPlayer { get; private set; } = null;

        public Action<GameObject> onEquipedObjectChange = (GameObject) => { /*Debug.Log("OnEquipedObjectChange");*/ };

		private void OnSelectedSlotChange(int index, Item item)
		{
			ChangeEquipedObject(item);
		}

		private void ClearEquipedObject()
		{
			if (equipedObject != null)
				Destroy(equipedObject);

			equipedObject = null;
		}
		private void InstantiateObject(Item item)
		{
			if (item != Item.None && item.Prefab != null)
				equipedObject = Instantiate(item.Prefab);
		}
		private void CheckIsEquipable()
		{
			if (equipedObject == null) return;

			var equipable = equipedObject.GetComponent<IEquipable>();
			if (equipable != null)
				equipable.Equip(rightHand, leftHand);
		}
		private void CheckIsOwnable()
		{
			if (equipedObject == null) return;

			var ownable = equipedObject.GetComponent<IOwnable>();
			if (ownable != null)
				ownable.SetOwner(OwnerPlayer);
		}

		protected virtual void Awake()
        {
            OwnerPlayer = GetComponent<Player>();
        }
		protected virtual void Start()
		{
			OwnerPlayer.ShortcutController.onSelectedSlotChange += OnSelectedSlotChange;
		}
		protected virtual void OnDestroy()
		{
			OwnerPlayer.ShortcutController.onSelectedSlotChange -= OnSelectedSlotChange;
		}

		protected virtual void ChangeEquipedObject(Item item)
		{
			SetEquipedObject(item);
		}

		public void SetEquipedObject(Item item)
		{
			ClearEquipedObject();
			InstantiateObject(item);
			CheckIsEquipable();
			CheckIsOwnable();

			onEquipedObjectChange.Invoke(equipedObject);
		}
	}
}
