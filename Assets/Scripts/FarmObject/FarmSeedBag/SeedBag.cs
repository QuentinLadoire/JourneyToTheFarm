using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class SeedBag : ActivableObject
	{
		[Header("SeedBag parameter")]
		[SerializeField] string seedName = "NoName";
		[SerializeField] string plantName = "NoName";
		[SerializeField] float growingDuration = 0.0f;
		[SerializeField] GameObject seedPreviewPrefab = null;

		TransportableObject transportableObject = null;

		FarmPlot farmPlot = null;
		PreviewObject seedPreview = null;

		RaycastHit hit;

		void OnSetHands(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
		}
		void OnHasMoved(Vector3 position)
		{
			if (isActived) return;

			if (seedPreview != null)
				if (Physics.Raycast(position + new Vector3(0.0f, 1.0f, 0.0f), Vector3.down, out hit))
				{
					seedPreview.transform.position = hit.point;
					seedPreview.transform.up = hit.normal;
				}

			if (IsActivable())
				seedPreview.SetBlueMat();
			else
				seedPreview.SetRedMat();
		}

		private void Awake()
		{
			transportableObject = GetComponent<TransportableObject>();
			transportableObject.onSetHands += OnSetHands;

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

			transportableObject.onSetHands -= OnSetHands;
			Player.OnHasMoved -= OnHasMoved;
		}

		public override bool IsActivable()
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
		public override void ApplyEffect()
		{
			farmPlot.SetSeed(seedName, growingDuration, plantName);
		}
		public override void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterPlantAPlant(true, GetDesiredAnimationSpeed());
		}
		public override void StopAnim(AnimationController animationController)
		{
			animationController.CharacterPlantAPlant(false);
		}
	}
}
