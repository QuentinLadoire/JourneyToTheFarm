using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class UsableController : MonoBehaviour
    {
		[SerializeField] Transform rightHandTransform = null;
		[SerializeField] Transform leftHandTransform = null;

		[SerializeField] ProgressBar progressBar = null;

        UsableItem usableItem = null;

		bool isActive = false;
		float currentDuration = 0.0f;

		CharacterController characterController = null;
		AnimationController animationController = null;
		Inventory inventory = null;

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

		void OnScroll(int index, ItemContainer itemContainer)
		{
			if (usableItem != null)
			{
				CancelItem();

				usableItem.Destroy();
			}

			if (itemContainer.Item != null && itemContainer.Item.prefab != null)
			{
				usableItem = Instantiate(itemContainer.Item.prefab).GetComponent<UsableItem>();
				usableItem.Init(rightHandTransform, leftHandTransform);
			}
		}

		void ItemInput()
		{
			if (Input.GetButtonDown("UseTool"))
				if (usableItem != null && characterController.IsIdle && usableItem.IsUsable())
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
			inventory = GetComponent<Inventory>();

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
