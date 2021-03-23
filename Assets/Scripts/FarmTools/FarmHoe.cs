using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FarmHoe : Tool
	{
		[Header("FarmHoe Paramater")]
		[SerializeField] GameObject farmPlotPrefab = null;
		[SerializeField] GameObject farmPlotPreviewPrefab = null;

		[SerializeField] float forwardOffset = 1.0f;

		GameObject farmPlotPreview = null;

		private void Awake()
		{
			farmPlotPreview = Instantiate(farmPlotPreviewPrefab);
		}
		private void Update()
		{
			Vector3 previewPosition = CharacterController.Position + CharacterController.Forward * forwardOffset;
			previewPosition.x = Mathf.RoundToInt(previewPosition.x);
			previewPosition.y = 0.0f;
			previewPosition.z = Mathf.RoundToInt(previewPosition.z);
			farmPlotPreview.transform.position = previewPosition;
		}

		public override bool IsUsable()
		{
			//todo
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
	}
}
