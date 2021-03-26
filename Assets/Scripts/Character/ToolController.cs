using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ToolController : MonoBehaviour
    {
		[SerializeField] Inventory inventory = null;

		[SerializeField] Transform handTransform = null;

		[SerializeField] ProgressBar progressBar = null;

        Tool tool = null;

		bool isActive = false;
		float currentDuration = 0.0f;

		CharacterController characterController = null;
		AnimationController animationController = null;

		void UseTool()
		{
			isActive = true;
			currentDuration = tool.duration;

			progressBar.SetVisible(true);
			tool.PlayAnim(animationController);
			tool.Use();
		}
		void UnuseTool()
		{
			tool.ApplyEffect();

			isActive = false;
			progressBar.SetVisible(false);
			tool.StopAnim(animationController);
			tool.Unuse();
		}
		void CancelTool()
		{
			if (!isActive) return;

			isActive = false;
			progressBar.SetVisible(false);
			tool.StopAnim(animationController);
			tool.Unuse();
		}

		void OnScroll(int index, Item item)
		{
			if (tool != null)
			{
				CancelTool();

				tool.Destroy();
			}

			if (item != null && item.type == ItemType.Tool)
			{
				var toolItem = item as ToolItem;
				tool = Instantiate(toolItem.prefab).GetComponent<Tool>();
				tool.transform.SetParent(handTransform, false);
			}
		}

		void ToolInput()
		{
			if (Input.GetButtonDown("ActivateTool"))
				if (tool != null && tool.IsUsable())
					UseTool();
		}
		void UpdateToolDuration()
		{
			if (!isActive || tool == null) return;

			if (currentDuration <= 0.0f)
				UnuseTool();

			currentDuration -= Time.deltaTime;
			progressBar.SetPercent( 1 - (currentDuration / tool.duration));
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
