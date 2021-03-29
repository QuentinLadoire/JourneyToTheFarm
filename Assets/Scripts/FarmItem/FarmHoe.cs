using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmHoe : UsableItem
	{
		[Header("FarmHoe Parameter")]
		[SerializeField] GameObject farmPlotPrefab = null;
		[SerializeField] GameObject farmPlotPreviewPrefab = null;

		PreviewObject farmPlotPreview = null;
		Transform leftHandTransform = null;

		void OnHasMoved(Vector3 position)
		{
			if (isUsed) return;

			farmPlotPreview.transform.position = position;

			if (IsUsable())
				farmPlotPreview.SetBlueMat();
			else
				farmPlotPreview.SetRedMat();
		}

		private void Awake()
		{
			farmPlotPreview = Instantiate(farmPlotPreviewPrefab).GetComponent<PreviewObject>();
			OnHasMoved(Player.RoundPosition);
		}
		private void Start()
		{
			Player.OnHasMoved += OnHasMoved;
		}
		private void Update()
		{
			transform.up = leftHandTransform.position - transform.position;
		}
		private void OnDestroy()
		{
			Player.OnHasMoved -= OnHasMoved;
		}

		public override void Init(Transform rightHand, Transform leftHand)
		{
			transform.SetParent(rightHand, false);
			leftHandTransform = leftHand;
		}
		public override bool IsUsable()
		{
			if (Physics.CheckBox(farmPlotPreview.transform.position + Vector3.up, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, LayerMask.GetMask("World", "Farm")))
				return false;

			return true;
		}
		public override void ApplyEffect()
		{
			var farmPlot = Instantiate(farmPlotPrefab);
			farmPlot.transform.position = farmPlotPreview.transform.position;
		}
		public override void PlayAnim(AnimationController animationController)
		{
			animationController.CharacterDiggingAnim(true);
		}
		public override void StopAnim(AnimationController animationController)
		{
			animationController.CharacterDiggingAnim(false);
		}
		public override void Destroy()
		{
            Destroy(gameObject);
			Destroy(farmPlotPreview.gameObject);
		}
	}
}
