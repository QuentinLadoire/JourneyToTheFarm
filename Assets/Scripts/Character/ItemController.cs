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

		void UseTool()
		{
			isActive = true;
			currentDuration = usableItem.duration;

			progressBar.SetVisible(true);
			usableItem.PlayAnim(animationController);
			usableItem.Use();
		}
		void UnuseTool()
		{
			usableItem.ApplyEffect();

			isActive = false;
			progressBar.SetVisible(false);
			usableItem.StopAnim(animationController);
			usableItem.Unuse();
		}
		void CancelTool()
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
				CancelTool();

				usableItem.Destroy();
			}

			if (item != null && item.type == ItemType.Tool)
			{
				var toolItem = item as ToolItem;
				usableItem = Instantiate(toolItem.prefab).GetComponent<UsableItem>();
				usableItem.transform.SetParent(handTransform, false);
			}
		}

		void ToolInput()
		{
			if (Input.GetButtonDown("ActivateTool"))
				if (usableItem != null && usableItem.IsUsable())
					UseTool();
		}
		void UpdateToolDuration()
		{
			if (!isActive || usableItem == null) return;

			if (currentDuration <= 0.0f)
				UnuseTool();

			currentDuration -= Time.deltaTime;
			progressBar.SetPercent( 1 - (currentDuration / usableItem.duration));
		}

		private void Awake()
		{
			animationController = GetComponent<AnimationController>();
			characterController = GetComponent<CharacterController>();

			characterController.onMoveEnter += CancelTool;

			if (inventory == null) Debug.Log("Inventory is null");
			else
			{
				inventory.onScroll += OnScroll;
			}
		}
		private void Update()
		{
			ToolInput();

			UpdateToolDuration();
		}
		private void OnDestroy()
		{
			characterController.onMoveEnter -= CancelTool;

			if (inventory != null)
			{
				inventory.onScroll -= OnScroll;
			}
		}
	}
}
