using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Chest : UsableBehaviour
	{
		[Header("Chest Settings")]
		[SerializeField] private LayerMask raycastLayer = -1;
		[SerializeField] private LayerMask overlapLayer = -1;
		[SerializeField] private GameObject chestPrefab = null;
		[SerializeField] private GameObject chestPreviewPrefab = null;

		private PreviewObject chestPreview = null;
		
		private bool IsConstructible()
		{
			var ray = new Ray((OwnerPlayer.transform.position + OwnerPlayer.transform.forward + new Vector3(0.0f, 2.0f, 0.0f)).RoundToInt(), Vector3.down);
			if (Physics.Raycast(ray, out RaycastHit hit, 5.0f, raycastLayer))
			{
				chestPreview.transform.position = hit.point;
				chestPreview.transform.forward = -OwnerPlayer.CharacterController.RoundForward;

				var center = chestPreview.transform.position + new Vector3(0.0f, 0.2355309f, 0.0f);
				var halfSize = new Vector3(1.234013f, 0.4710618f, 0.7313852f) * 0.5f;
				return (hit.transform.CompareTag("Constructible") &&
					!Physics.CheckBox(center, halfSize, chestPreview.transform.rotation, overlapLayer));
			}

			return false;
		}
		private void Updatefeedback()
		{
			if (IsUsable)
			{
				chestPreview.SetBlueColor();

				InteractionText.SetText("Press E to Place");
				InteractionText.SetActive(true);
			}
			else
			{
				chestPreview.SetRedColor();

				InteractionText.SetActive(false);
			}
		}

		protected override bool CheckIsUsable()
		{
			return IsConstructible();
		}

		protected override void Awake()
		{
			base.Awake();

			if (chestPreviewPrefab != null)
				chestPreview = Instantiate(chestPreviewPrefab).GetComponent<PreviewObject>();
		}
		protected override void Update()
		{
			base.Update();

			Updatefeedback();
		}
		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (chestPreview != null)
				chestPreview.Destroy();
		}

		public override void Use()
		{
			if (chestPrefab != null)
			{
				Instantiate(chestPrefab, chestPreview.transform.position, chestPreview.transform.rotation);
				OwnerPlayer.ShortcutController.ConsumeSelectedItem();
			}
		}
	}
}
