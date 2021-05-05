using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class HandedChest : SimpleObject, IHandable, IUsable
	{
		public float Duration => duration;
		public float AnimationDuration => animationDuration;
		public float AnimationMultiplier => animationMultiplier;

		[SerializeField] float duration = 0.0f;
		[SerializeField] float animationDuration = 0.0f;
		[SerializeField] float animationMultiplier = 1.0f;

		[SerializeField] GameObject chestPrefab = null;
		[SerializeField] GameObject chestPreviewPrefab = null;

		RaycastHit hit;
		PreviewObject chestPreview = null;

		public void SetHanded(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public bool IsUsable()
		{
			return IsConstructible();
		}
		public void Use()
		{
			if (chestPrefab != null)
			{
				Instantiate(chestPrefab, chestPreview.transform.position, chestPreview.transform.rotation);
				Player.RemoveItem("Chest", 1);
			}
		}
		public void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterPlacing(true, animationController.GetDesiredAnimationSpeed(duration, animationDuration, animationMultiplier));
		}
		public void StopAnim(AnimationController animationController)
		{
			animationController.CharacterPlacing(false);
		}

		bool IsConstructible()
		{
			var center = chestPreview.transform.position + new Vector3(0.0f, 0.2355309f, 0.0f);
			var halfSize = new Vector3(1.234013f, 0.4710618f, 0.7313852f) / 2.0f;
			return (hit.transform.CompareTag("Constructible") &&
				!Physics.CheckBox(center, halfSize, chestPreview.transform.rotation, GameManager.GetConstructibleOverlapMask()));
		}

		protected override void Awake()
		{
			base.Awake();

			if (chestPreviewPrefab != null)
				chestPreview = Instantiate(chestPreviewPrefab).GetComponent<PreviewObject>();
		}
		private void Update()
		{
			if (chestPreview == null) return;

			var ray = new Ray((Player.Position + Player.Forward + new Vector3(0.0f, 2.0f, 0.0f)).RoundToInt(), Vector3.down);
			if (Physics.Raycast(ray, out hit, 5.0f, GameManager.GetConstructiblRaycastMask()))
			{
				chestPreview.transform.position = hit.point;
				chestPreview.transform.forward = -Player.RoundForward;

				if (IsConstructible())
					chestPreview.SetBlueColor();
				else
					chestPreview.SetRedColor();
			}
		}
		private void OnDestroy()
		{
			if (chestPreview != null)
				chestPreview.Destroy();
		}

		private void OnDrawGizmos()
		{
			Gizmos.matrix = chestPreview.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(new Vector3(0.0f, 0.2355309f, 0.0f), new Vector3(1.234013f, 0.4710618f, 0.7313852f));
		}
	}
}
