using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

namespace JTTF.Behaviour
{
	[RequireComponent(typeof(NetworkObject))]
	public class CustomNetworkBehaviour : NetworkBehaviour
	{
		[SerializeField] private bool isActiveOnAwake = true;

		private bool firstFrame = true;

		protected virtual void FirstFrameUpdate() { }

		protected virtual void Awake()
		{
			SetActive(isActiveOnAwake);
		}
		protected virtual void Start() { }
		protected virtual void Update()
		{
			if (firstFrame)
			{
				FirstFrameUpdate();
				firstFrame = false;
			}
		}
		protected virtual void OnDestroy() { }

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
