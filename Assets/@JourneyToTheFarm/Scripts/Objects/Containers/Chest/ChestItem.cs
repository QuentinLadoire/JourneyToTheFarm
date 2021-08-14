using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class ChestItem : CustomBehaviour, IEquipable, IUsable, IOwnable
	{
		public Player OwnerPlayer { get; private set; }

		public float Duration => duration;
		public ActionType ActionType => ActionType.Place;
		public bool IsUsable => isUsable;

		[Header("Chest Parameters")]
		[SerializeField] float duration = 0.0f;
		[SerializeField] LayerMask raycastLayer = -1;
		[SerializeField] LayerMask overlapLayer = -1;
		[SerializeField] GameObject chestPrefab = null;
		[SerializeField] GameObject chestPreviewPrefab = null;

		RaycastHit hit;
		bool isUsable = false;
		PreviewObject chestPreview = null;
		PlayerInteractionText interactionText = null;

		bool IsConstructible()
		{
			var center = chestPreview.transform.position + new Vector3(0.0f, 0.2355309f, 0.0f);
			var halfSize = new Vector3(1.234013f, 0.4710618f, 0.7313852f) / 2.0f;
			return (hit.transform.CompareTag("Constructible") &&
				!Physics.CheckBox(center, halfSize, chestPreview.transform.rotation, overlapLayer));
		}
		void CheckIsUsable()
		{
			if (IsConstructible())
			{
				interactionText.SetText("Press E to Place");
				interactionText.SetActive(true);

				isUsable = true;
			}
			else
			{
				interactionText.SetActive(false);

				isUsable = false;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			interactionText = CanvasManager.GamePanel.PlayerPanel.PlayerInteractionText;

			if (chestPreviewPrefab != null)
				chestPreview = Instantiate(chestPreviewPrefab).GetComponent<PreviewObject>();
		}
		private void Update()
		{
			if (chestPreview == null) return;

			var ray = new Ray((OwnerPlayer.transform.position + OwnerPlayer.transform.forward + new Vector3(0.0f, 2.0f, 0.0f)).RoundToInt(), Vector3.down);
			if (Physics.Raycast(ray, out hit, 5.0f, raycastLayer))
			{
				chestPreview.transform.position = hit.point;
				chestPreview.transform.forward = -OwnerPlayer.CharacterController.RoundForward;

				if (isUsable)
					chestPreview.SetBlueColor();
				else
					chestPreview.SetRedColor();
			}

			CheckIsUsable();
		}
		private void OnDestroy()
		{
			if (chestPreview != null)
				chestPreview.Destroy();

			if (interactionText != null)
				interactionText.SetActive(false);
		}

		public void SetOwner(Player owner)
		{
			OwnerPlayer = owner;
		}

		public void Equip(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public void Use()
		{
			if (chestPrefab != null)
			{
				Instantiate(chestPrefab, chestPreview.transform.position, chestPreview.transform.rotation);
				OwnerPlayer.ShortcutController.ConsumeSelectedItem();
			}
		}
	}
}
