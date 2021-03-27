using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
    public class Player : MonoBehaviour
    {
		static Player instance = null;

		public static Vector3 Position { get => instance.transform.position; }
		public static Vector3 Forward { get => instance.transform.forward; }
		public static Vector3 RoundPosition { get => instance.characterController.RoundPosition; }

		public static Action<Vector3> OnHasMoved { get => instance.characterController.onHasMoved; set => instance.characterController.onHasMoved = value; }

        CharacterController characterController = null;
        ItemController itemController = null;
        AnimationController animationController = null;

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			itemController = GetComponent<ItemController>();
			animationController = GetComponent<AnimationController>();

			instance = this;
		}
	}
}
