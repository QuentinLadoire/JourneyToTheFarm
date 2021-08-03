using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class ShortcutInventoryController : MonoBehaviour
	{
		public Player OwnerPlayer { get; private set; }

		public Action<int> onScroll = (int index) => { /*Debug.Log("OnScroll");*/ };
		public Action onInventoryChange = () => { /*Debug.Log("OnRefreshInventory");*/ };
		public Action<GameObject> onEquipedObjectChange = (GameObject) => { /*Debug.Log("OnEquipedObjectChange");*/ };

		public Inventory Inventory => inventory;

		[SerializeField] Transform rightHand = null;
		[SerializeField] Transform leftHand = null;

		int currentIndex = 0;
		GameObject equipedObject = null;
		readonly Inventory inventory = new Inventory(10);

		public bool AddItem(Item item)
		{
			var value = inventory.AddItem(item);
			ChangeEquipedObject();
			onInventoryChange.Invoke();

			return value;
		}
		public bool RemoveItem(Item item)
		{
			var value = inventory.RemoveItem(item);
			ChangeEquipedObject();
			onInventoryChange.Invoke();

			return value;
		}

		void ClearEquipedObject()
		{
			if (equipedObject != null)
				Destroy(equipedObject);
		}
		void InstantiateObject()
		{
			if (inventory.ItemArray[currentIndex] != null)
			{
				equipedObject = Instantiate(inventory.ItemArray[currentIndex].Prefab);

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
		
		void ChangeEquipedObject()
		{
			ClearEquipedObject();
			InstantiateObject();
			CheckIsOwnable();
			CheckIsEquipable();
		}

		void ScrollUp()
		{
			currentIndex--;
			if (currentIndex == -1)
				currentIndex = inventory.SizeMax - 1;

			onScroll.Invoke(currentIndex);

			ChangeEquipedObject();
		}
		void ScrollDown()
		{
			currentIndex++;
			if (currentIndex == inventory.SizeMax)
				currentIndex = 0;

			onScroll.Invoke(currentIndex);

			ChangeEquipedObject();
		}
		void ProcessInput()
		{
			var delta = Input.GetAxisRaw("ScrollShortcut");
			if (delta > 0.0f)
				ScrollUp();
			else if (delta < 0.0f)
				ScrollDown();
		}

		private void Awake()
		{
			OwnerPlayer = GetComponent<Player>();
		}
		private void Start()
		{
			inventory.AddItem(new Item("Shovel", ItemType.Tool, 1));
			inventory.AddItem(new Item("Axe", ItemType.Tool, 1));
			inventory.AddItem(new Item("Pickaxe", ItemType.Tool, 1));
			inventory.AddItem(new Item("Scythe", ItemType.Tool, 1));
			inventory.AddItem(new Item("WheatSeed", ItemType.Seed, 20));
			inventory.AddItem(new Item("Chest", ItemType.Container, 1));

			CanvasManager.GamePanel.InitShortcutInventory(this);

			ChangeEquipedObject();
		}
		private void Update()
		{
			ProcessInput();
		}
	}
}
