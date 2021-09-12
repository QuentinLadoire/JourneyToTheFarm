using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JTTF.UI;
using JTTF.Enum;
using JTTF.Interface;

namespace JTTF.Behaviour
{
	public class UsableBehaviour : EquipableBehaviour, IUsable
	{
		[Header("UsableBehaviour Settings")]
		[SerializeField] private float actionDuration = 0.0f;
		[SerializeField] private ActionType actionType = ActionType.None;

		private bool isUsable = false;
		private PlayerInteractionText interactionText = null;

		public bool IsUsable => isUsable;
		public float ActionDuration => actionDuration;
		public ActionType ActionType => actionType;
		public PlayerInteractionText InteractionText => interactionText;

		protected override void Awake()
		{
			base.Awake();

			interactionText = CanvasManager.GamePanel.PlayerPanel.PlayerInteractionText;
		}
		protected override void Update()
		{
			base.Update();

			isUsable = CheckIsUsable();
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (interactionText != null)
				interactionText.SetActive(false);
		}

		protected virtual bool CheckIsUsable()
		{
			return false;
		}

		public virtual void Use()
		{
			Debug.LogWarning("The Mehtod Use of " + this.GetType().ToString() + ", is not Implemented.");
		}
	}
}
