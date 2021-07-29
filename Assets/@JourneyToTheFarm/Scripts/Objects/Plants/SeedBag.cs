using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class SeedBag : CustomBehaviour, IHandable, IUsable
	{
		public float Duration => duration;
		public float AnimationDuration => animationDuration;
		public float AnimationMultiplier => animationMultiplier;

		[SerializeField] float duration = 0.0f;
		[SerializeField] float animationDuration = 0.0f;
		[SerializeField] float animationMultiplier = 1.0f;

		[Header("SeedBag parameter")]
		[SerializeField] string seedName = "NoName";
		[SerializeField] string plantName = "NoName";
		[SerializeField] float growingDuration = 0.0f;
		[SerializeField] GameObject seedPreviewPrefab = null;

		FarmPlot farmPlot = null;
		PreviewObject seedPreview = null;

		bool IsPlantable()
		{
			if (Physics.Raycast(Player.RoundPosition + new Vector3(0.0f, 2.0f, 0.0f), Vector3.down, out RaycastHit hit, 5.0f, GameManager.GetPlantableRaycastMask()))
			{
				farmPlot = hit.collider.GetComponent<FarmPlot>();
				if (farmPlot != null)
					return !farmPlot.HasSeed;
			}

			return false;
		}

		void OnHasMoved()
		{
			if (Physics.Raycast(Player.RoundPosition + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out RaycastHit hit, 5.0f, GameManager.GetConstructiblRaycastMask()))
			{
				seedPreview.transform.position = hit.point;
				seedPreview.transform.up = hit.normal;
			}

			if (IsPlantable())
				seedPreview.SetBlueColor();
			else
				seedPreview.SetRedColor();
		}

		public void SetHanded(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public bool IsUsable()
		{
			return IsPlantable();
		}
		public void Use()
		{
			farmPlot.SetSeed(seedName, growingDuration, plantName);
		}

		public void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterPlantAPlant(true, animationController.GetDesiredAnimationSpeed(duration, animationDuration, animationMultiplier));
		}
		public void StopAnim(AnimationController animationController)
		{
			animationController.CharacterPlantAPlant(false);
		}

		protected override void Awake()
		{
			base.Awake();

			seedPreview = Instantiate(seedPreviewPrefab).GetComponent<PreviewObject>();
			OnHasMoved();
		}
		private void Start()
		{
			Player.OnHasMoved += OnHasMoved;
		}
		private void OnDestroy()
		{
			if (seedPreview != null)
				Destroy(seedPreview.gameObject);

			Player.OnHasMoved -= OnHasMoved;
		}
	}
}
