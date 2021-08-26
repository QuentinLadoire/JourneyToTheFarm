using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Chest : UsableBehaviour
	{
		[Header("Chest Settings")]
		[SerializeField] LayerMask raycastLayer = -1;
		[SerializeField] LayerMask overlapLayer = -1;
		[SerializeField] GameObject chestPrefab = null;
		[SerializeField] GameObject chestPreviewPrefab = null;

		RaycastHit hit;
		PreviewObject chestPreview = null;
		PlayerInteractionText interactionText = null;

		bool IsConstructible()
		{
			var center = chestPreview.transform.position + new Vector3(0.0f, 0.2355309f, 0.0f);
			var halfSize = new Vector3(1.234013f, 0.4710618f, 0.7313852f) / 2.0f;
			return (hit.transform.CompareTag("Constructible") &&
				!Physics.CheckBox(center, halfSize, chestPreview.transform.rotation, overlapLayer));
		}
		protected override bool CheckIsUsable()
		{
			interactionText.SetActive(false);

			if (IsConstructible())
			{
				interactionText.SetText("Press E to Place");
				interactionText.SetActive(true);

				return true;
			}

			return false;
		}

		protected override void Awake()
		{
			base.Awake();

			interactionText = CanvasManager.GamePanel.PlayerPanel.PlayerInteractionText;

			if (chestPreviewPrefab != null)
				chestPreview = Instantiate(chestPreviewPrefab).GetComponent<PreviewObject>();
		}
		protected override void Update()
		{
			if (chestPreview == null) return;

			var ray = new Ray((OwnerPlayer.transform.position + OwnerPlayer.transform.forward + new Vector3(0.0f, 2.0f, 0.0f)).RoundToInt(), Vector3.down);
			if (Physics.Raycast(ray, out hit, 5.0f, raycastLayer))
			{
				chestPreview.transform.position = hit.point;
				chestPreview.transform.forward = -OwnerPlayer.CharacterController.RoundForward;

				if (IsUsable)
					chestPreview.SetBlueColor();
				else
					chestPreview.SetRedColor();
			}

			base.Update();
		}
		private void OnDestroy()
		{
			if (chestPreview != null)
				chestPreview.Destroy();

			if (interactionText != null)
				interactionText.SetActive(false);
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
