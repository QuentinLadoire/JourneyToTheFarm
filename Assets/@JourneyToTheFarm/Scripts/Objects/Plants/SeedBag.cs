using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class SeedBag : CustomBehaviour, IEquipable, IUsable, IOwnable
	{
		public Player OwnerPlayer { get; private set; }

		public float Duration => duration;
		public ActionType ActionType => ActionType.Plant;
		public bool IsUsable => isUsable;

		[Header("SeedBag parameter")]
		public LayerMask raycastLayer = -1;
		public LayerMask plantableLayer = -1;
		public float duration = 0.0f;
		public string seedName = "NoName";
		public string plantName = "NoName";
		public float growingDuration = 0.0f;
		public GameObject seedPreviewPrefab = null;

		bool isUsable = false;
		FarmPlot farmPlot = null;
		PreviewObject seedPreview = null;
		PlayerInteractionText interactionText = null;

		bool IsPlantable()
		{
			if (Physics.Raycast(OwnerPlayer.CharacterController.RoundPosition + new Vector3(0.0f, 2.0f, 0.0f), Vector3.down, out RaycastHit hit, 5.0f, plantableLayer))
			{
				farmPlot = hit.collider.GetComponent<FarmPlot>();
				if (farmPlot != null)
					return !farmPlot.HasSeed;
			}

			return false;
		}
		void CheckIsUsable()
		{
			if (IsPlantable())
			{
				interactionText.SetText("Press E to Plant");
				interactionText.SetActive(true);

				isUsable = true;
			}
			else
			{
				interactionText.SetActive(false);

				isUsable = false;
			}
		}
		void UpdatePreview()
		{
			if (OwnerPlayer == null) return;

			if (Physics.Raycast(OwnerPlayer.CharacterController.RoundPosition + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out RaycastHit hit, 5.0f, raycastLayer))
			{
				seedPreview.transform.position = hit.point;
				seedPreview.transform.up = hit.normal;
			}

			if (isUsable)
				seedPreview.SetBlueColor();
			else
				seedPreview.SetRedColor();
		}

		protected override void Awake()
		{
			base.Awake();

			interactionText = CanvasManager.GamePanel.PlayerPanel.PlayerInteractionText;

			seedPreview = Instantiate(seedPreviewPrefab).GetComponent<PreviewObject>();
		}
		private void Update()
		{
			CheckIsUsable();

			UpdatePreview();
		}
		private void OnDestroy()
		{
			if (seedPreview != null)
				Destroy(seedPreview.gameObject);

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
			farmPlot.SetSeed(seedName, growingDuration, plantName);
			OwnerPlayer.ShortcutController.ConsumeSelectedItem();
		}
	}
}
