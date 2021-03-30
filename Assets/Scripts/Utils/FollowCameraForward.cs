using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class FollowCameraForward : MonoBehaviour
	{
		new Camera camera = null;

		private void Start()
		{
			camera = Camera.main;
		}
		private void Update()
		{
			if (camera != null)
				transform.forward = camera.transform.forward;
		}
	}
}
