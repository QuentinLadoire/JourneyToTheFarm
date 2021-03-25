using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class ToolController : MonoBehaviour
    {
		[SerializeField] ProgressBar progressBar = null;

        public Tool tool = null;

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
		}
		private void Start()
		{
			characterController.onMoveEnter += CancelTool;
		}
		private void Update()
		{
			ToolInput();

			UpdateToolDuration();
		}
	}
}
