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

		[SerializeField] float forwardOffset = 1.0f;

		FarmPlotPreview farmPlotPreview = null;
		bool isUsed = false;

		void HasMove()
		{
			if (IsUsable())
				farmPlotPreview.SetBlueMat();
			else
				farmPlotPreview.SetRedMat();
		}

		private void Awake()
		{
			farmPlotPreview = Instantiate(farmPlotPreviewPrefab).GetComponent<FarmPlotPreview>();
		}
		private void Update()
		{
			if (isUsed) return;

			Vector3 previewPosition = CharacterController.Position + CharacterController.Forward * forwardOffset;
			previewPosition.x = Mathf.RoundToInt(previewPosition.x);
			previewPosition.y = 0.0f;
			previewPosition.z = Mathf.RoundToInt(previewPosition.z);

			bool hasMove = false;
			if (previewPosition != farmPlotPreview.transform.position)
				hasMove = true;

			farmPlotPreview.transform.position = previewPosition;

			if (hasMove)
				HasMove();
		}

		public override void Init(Item item, Transform parent)
		{
			transform.SetParent(parent, false);
		}
		public override bool IsUsable()
		{
			if (Physics.CheckBox(farmPlotPreview.transform.position + new Vector3(0.0f, 1.0f, 0.0f), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, LayerMask.GetMask("World", "Farm")))
				return false;

			return true;
		}
		public override void Use()
		{
			isUsed = true;
		}
		public override void Unuse()
		{
			isUsed = false;
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
