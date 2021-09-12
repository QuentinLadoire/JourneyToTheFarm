using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF.Behaviour
{
	public class CustomBehaviour : MonoBehaviour
	{
		[SerializeField] bool isActiveOnAwake = true;

		private bool firstFrame = true;

		protected virtual void FirstFrameUpdate()
		{

		}

		protected virtual void Awake()
		{
			SetActive(isActiveOnAwake);
		}
		protected virtual void Start()
		{

		}
		protected virtual void Update()
		{
			if (firstFrame)
			{
				FirstFrameUpdate();
				firstFrame = false;
			}
		}
		protected virtual void OnDestroy()
		{

		}

		public void SetActive(bool value)
		{
			isActiveOnAwake = true;
			gameObject.SetActive(value);
		}
		public void SetParent(Transform parent, bool worldPositionStay)
		{
			transform.SetParent(parent, worldPositionStay);
		}
		public void Destroy()
		{
			Destroy(gameObject);
		}
	}
}
