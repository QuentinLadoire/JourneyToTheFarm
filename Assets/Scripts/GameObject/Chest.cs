using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class Chest : SimpleObject, IInteractable
	{
		public float Duration => duration;
		public float AnimationDuration => animationDuration;
		public float AnimationMultiplier => animationMultipler;

		[SerializeField] float duration = 0.0f;
		[SerializeField] float animationDuration = 0.0f;
		[SerializeField] float animationMultipler = 1.0f;

		[SerializeField] GameObject chestLid = null;
		[SerializeField] GameObject interactableImage = null;

		Inventory inventory = null;
		Coroutine openingCoroutine = null;
		Coroutine closingCoroutine = null;

		public bool IsInteractable()
		{
			return inventory != null;
		}

		public void Select()
		{
			if (interactableImage != null)
				interactableImage.SetActive(true);
		}
		public void Deselect()
		{
			if (interactableImage != null)
				interactableImage.SetActive(false);
		}
		public void Interact()
		{
			OpenInventory();
		}

		public void PlayAnim(AnimationController animationController)
		{
			LaunchOpeningAnim(animationController.GetDesiredAnimationSpeed(duration, animationDuration, animationMultipler));
		}
		public void StopAnim(AnimationController animationController)
		{
			LaunchClosingAnim(animationController.GetDesiredAnimationSpeed(duration, animationDuration, animationMultipler));
		}

		IEnumerator OpeningAnimation(float speed)
		{
			while (transform.eulerAngles.x > -55.0f)
			{
				float angle = chestLid.transform.eulerAngles.x + Time.deltaTime * speed;
				chestLid.transform.eulerAngles -= new Vector3(angle, 0.0f, 0.0f);

				yield return null;
			}
		}
		IEnumerator ClosingAnimation(float speed)
		{
			while (transform.eulerAngles.x < 0.0f)
			{
				float angle = chestLid.transform.eulerAngles.x + Time.deltaTime * speed;
				chestLid.transform.eulerAngles += new Vector3(angle, 0.0f, 0.0f);

				yield return null;
			}
		}

		void LaunchOpeningAnim(float speed)
		{
			if (closingCoroutine != null)
				StopCoroutine(closingCoroutine);

			if (openingCoroutine == null)
				openingCoroutine = StartCoroutine(OpeningAnimation(speed));
		}
		void LaunchClosingAnim(float speed)
		{
			if (openingCoroutine != null)
				StopCoroutine(openingCoroutine);

			if (closingCoroutine == null)
				closingCoroutine = StartCoroutine(ClosingAnimation(speed));
		}

		void OpenInventory()
		{
			Debug.Log("OpenInventory");
		}

		private void Awake()
		{
			inventory = GetComponent<Inventory>();
		}
	}
}
