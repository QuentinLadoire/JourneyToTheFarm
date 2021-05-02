using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class HandedChest : SimpleObject, IHandable, IUsable
	{
		[SerializeField] GameObject chestPrefab = null;
		[SerializeField] GameObject chestPreviewPrefab = null;

		PreviewObject chestPreview = null;

		public float Duration => throw new System.NotImplementedException();

		public float AnimationDuration => throw new System.NotImplementedException();

		public float AnimationMultiplier => throw new System.NotImplementedException();

		public void SetHanded(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public bool IsUsable()
		{
			Debug.Log("IsUsable is not implemented");

			return false;
		}
		public void Use()
		{
			Debug.Log("Use is not implemented");
		}
		public void PlayAnim(AnimationController animationController)
		{
			Debug.Log("PlayAnim is not implemented");
		}
		public void StopAnim(AnimationController animationController)
		{
			Debug.Log("StopAnim is not implemented");
		}

		private void Awake()
		{
			if (chestPreviewPrefab != null)
				chestPreview = Instantiate(chestPreviewPrefab).GetComponent<PreviewObject>();
		}
		private void Update()
		{
			if (chestPreview == null) return;

			RaycastHit hit;
			if (Physics.Raycast(Player.RoundPosition + Player.RoundForward + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out hit))
			{
				chestPreview.transform.position = hit.point;
				chestPreview.transform.forward = -Player.RoundForward;

				if (hit.transform.CompareTag("Ground"))
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
	}
}
