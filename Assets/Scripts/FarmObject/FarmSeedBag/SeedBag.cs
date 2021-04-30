using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class SeedBag : SimpleObject, IHandable, IUsable
	{
		public float Duration { get => duration; }
		public float AnimationDuration { get => animationDuration; }
		public float AnimationMultiplier { get => animationMultiplier; }

		[SerializeField] float duration = 0.0f;
		[SerializeField] float animationDuration = 0.0f;
		[SerializeField] float animationMultiplier = 1.0f;

		[Header("SeedBag parameter")]
		[SerializeField] string seedName = "NoName";
		[SerializeField] string plantName = "NoName";
		[SerializeField] float growingDuration = 0.0f;
		[SerializeField] GameObject seedPreviewPrefab = null;

		bool isUsed = false;
		FarmPlot farmPlot = null;
		PreviewObject seedPreview = null;

		RaycastHit hit;

		void OnHasMoved(Vector3 position)
		{
			if (isUsed) return;

			if (seedPreview != null)
				if (Physics.Raycast(position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out hit))
				{
					seedPreview.transform.position = hit.point;
					seedPreview.transform.up = hit.normal;
				}

			if (IsUsable())
				seedPreview.SetBlueMat();
			else
				seedPreview.SetRedMat();
		}

		public void SetHanded(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}

		public bool IsUsable()
		{
			var colliders = Physics.OverlapBox(seedPreview.transform.position + new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0.4f, 0.5f, 0.4f));
			foreach (var collider in colliders)
				if (collider.CompareTag("FarmPlot"))
				{
					farmPlot = collider.GetComponent<FarmPlot>();
					return !farmPlot.HasSeed;
				}

			return false;
		}
		public void Use()
		{
			isUsed = true;
		}
		public void Unuse()
		{
			isUsed = false;
		}
		public void ApplyEffect()
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

		private void Awake()
		{
			seedPreview = Instantiate(seedPreviewPrefab).GetComponent<PreviewObject>();
			OnHasMoved(Player.RoundPosition);
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
