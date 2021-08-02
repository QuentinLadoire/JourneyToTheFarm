using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class ChestItem : CustomBehaviour, IEquipable, IUsable
	{
		public float Duration => duration;
		public ActionType ActionType => ActionType.Place;

		[Header("Chest Parameters")]
		public float duration = 0.0f;
		public LayerMask raycastLayer = -1;
		public LayerMask overlapLayer = -1;
		public GameObject chestPrefab = null;
		public GameObject chestPreviewPrefab = null;

		RaycastHit hit;
		PreviewObject chestPreview = null;

		public void Equip(Transform rightHand, Transform leftHand)
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

			}
		}

		bool IsConstructible()
		{
			var center = chestPreview.transform.position + new Vector3(0.0f, 0.2355309f, 0.0f);
			var halfSize = new Vector3(1.234013f, 0.4710618f, 0.7313852f) / 2.0f;
			return (hit.transform.CompareTag("Constructible") &&
				!Physics.CheckBox(center, halfSize, chestPreview.transform.rotation, overlapLayer));
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
			if (Physics.Raycast(ray, out hit, 5.0f, raycastLayer))
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
	}
}
