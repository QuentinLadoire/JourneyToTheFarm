using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable IDE0044

namespace JTTF
{
    public class EquipableController : CustomNetworkBehaviour
    {
        [SerializeField] private Transform rightHand = null;
        [SerializeField] private Transform leftHand = null;

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

		private void ChangeEquipedObject(Item item)
		{
			ClearEquipedObject();
			InstantiateObject(item);
			CheckIsEquipable();
			CheckIsOwnable();

			onEquipedObjectChange.Invoke(equipedObject);
		}

		protected override void Awake()
        {
			base.Awake();

            OwnerPlayer = GetComponent<Player>();
        }
		public override void NetworkStart()
		{
			base.NetworkStart();

			if (!(this.IsClient && this.IsLocalPlayer))
			{
				this.enabled = false;
				return;
			}
		}
		protected override void Start()
		{
			base.Start();

			OwnerPlayer.ShortcutController.onSelectedSlotChange += OnSelectedSlotChange;
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

			OwnerPlayer.ShortcutController.onSelectedSlotChange -= OnSelectedSlotChange;
		}
	}
}
