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

		AnimationController animationController = null;

		void UseTool()
		{
			isActive = true;
			currentDuration = tool.duration;

			progressBar.SetVisible(true);
			tool.PlayAnim(animationController);
		}
		void UnuseTool()
		{
			tool.ApplyEffect();

			isActive = false;
			progressBar.SetVisible(false);
			tool.StopAnim(animationController);
		}
		void CancelTool()
		{
			isActive = false;
			progressBar.SetVisible(false);
			tool.StopAnim(animationController);
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
		}
		private void Update()
		{
			ToolInput();

			UpdateToolDuration();
		}
	}
}
