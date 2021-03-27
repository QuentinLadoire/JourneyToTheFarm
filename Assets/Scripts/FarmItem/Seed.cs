using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Seed : UsableItem
	{
		[Header("Seed Parameter")]
		[SerializeField] SpriteRenderer spriteRenderer = null;

		void SetSeedIcon(Sprite sprite)
		{
			spriteRenderer.sprite = sprite;
		}

		public override void Init(Item item, Transform parent)
		{
			SetSeedIcon(item.sprite);
			transform.SetParent(parent, false);
		}
		public override bool IsUsable()
		{
			Debug.Log("IsUsable not implemented");

			return false;
		}
		public override void Use()
		{
			Debug.Log("Use not implemented");
		}
		public override void Unuse()
		{
			Debug.Log("Unuse not implemented");
		}
		public override void ApplyEffect()
		{
			Debug.Log("ApplyEffect not implemented");
		}
		public override void PlayAnim(AnimationController animationController)
		{
			Debug.Log("PlayAnim not implemented");
		}
		public override void StopAnim(AnimationController animationController)
		{
			Debug.Log("StopAnim not implemented");
		}
		public override void Destroy()
		{
			Destroy(gameObject);
		}
	}
}
