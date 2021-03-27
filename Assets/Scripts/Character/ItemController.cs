using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ItemController : MonoBehaviour
    {
		[SerializeField] Inventory inventory = null;

		[SerializeField] Transform handTransform = null;

		[SerializeField] ProgressBar progressBar = null;

        UsableItem usableItem = null;

		bool isActive = false;
		float currentDuration = 0.0f;

		CharacterController characterController = null;
		AnimationController animationController = null;

		void UseItem()
		{
			isActive = true;
			currentDuration = usableItem.duration;

			progressBar.SetVisible(true);
			usableItem.PlayAnim(animationController);
			usableItem.Use();
		}
		void UnuseItem()
		{
			usableItem.ApplyEffect();

			isActive = false;
			progressBar.SetVisible(false);
			usableItem.StopAnim(animationController);
			usableItem.Unuse();
		}
		void CancelItem()
		{
			if (!isActive) return;

			isActive = false;
			progressBar.SetVisible(false);
			usableItem.StopAnim(animationController);
			usableItem.Unuse();
		}

		void OnScroll(int index, Item item)
		{
			if (usableItem != null)
			{
				CancelItem();

				usableItem.Destroy();
			}

			if (item != null)
			{
				usableItem = Instantiate(item.prefab).GetComponent<UsableItem>();
				usableItem.Init(item, handTransform);
			}
		}

		void ItemInput()
		{
			if (Input.GetButtonDown("ActivateTool"))
				if (usableItem != null && usableItem.IsUsable())
					UseItem();
		}
		void UpdateItemDuration()
		{
			if (!isActive || usableItem == null) return;

			if (currentDuration <= 0.0f)
				UnuseItem();

			currentDuration -= Time.deltaTime;
			progressBar.SetPercent( 1 - (currentDuration / usableItem.duration));
		}

		private void Awake()
		{
			animationController = GetComponent<AnimationController>();
			characterController = GetComponent<CharacterController>();

			characterController.onMoveEnter += CancelItem;

			if (inventory == null) Debug.Log("Inventory is null");
			else
			{
				inventory.onScroll += OnScroll;
			}
		}
		private void Update()
		{
			ItemInput();

			UpdateItemDuration();
		}
		private void OnDestroy()
		{
			characterController.onMoveEnter -= CancelItem;

			if (inventory != null)
			{
				inventory.onScroll -= OnScroll;
			}
		}
	}
}
