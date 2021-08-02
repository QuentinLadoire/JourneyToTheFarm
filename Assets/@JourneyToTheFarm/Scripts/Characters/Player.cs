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
		public static Vector3 Right => instance.transform.right;
		public static Vector3 RoundPosition { get => instance.characterController.RoundPosition; }
		public static Vector3 RoundForward { get => instance.transform.forward.RoundToInt(); }

		public static bool HasControl { get => instance.hasControl; }
		public static Action OnActiveControl { get => instance.onActiveControl; set => instance.onActiveControl = value; }
		public static Action OnDesactiveControl { get => instance.onDesactiveControl; set => instance.onDesactiveControl = value; }

		//CharacterController
		public static Action OnMoveEnter { get => instance.characterController.onMoveEnter; set => instance.characterController.onMoveEnter = value; }
		public static Action<Vector3> OnMove { get => instance.characterController.onMove; set => instance.characterController.onMove = value; }
		public static Action OnMoveExit { get => instance.characterController.onMoveExit; set => instance.characterController.onMoveExit = value; }
		public static Action OnHasMoved { get => instance.characterController.onHasMoved; set => instance.characterController.onHasMoved = value; }

		public static bool IsIdle => instance.characterController.IsIdle;
		public static Vector3 Direction => instance.characterController.Direction;

		//EquipableObjectController
		public static Action<GameObject> OnHandedObjectChange { get => instance.equipableObjectController.onEquipedObjectChange; set => instance.equipableObjectController.onEquipedObjectChange = value; }

		//UsableObjectController
		public static Action<ActionType, float> OnStartToUseObject { get => instance.usableObjectController.onStartToUseObject; set => instance.usableObjectController.onStartToUseObject = value; }
		public static Action<ActionType, float> OnUseObject { get => instance.usableObjectController.onUseObject; set => instance.usableObjectController.onUseObject = value; }
		public static Action<ActionType, float> OnStopToUseObject { get => instance.usableObjectController.onStopToUseObject; set => instance.usableObjectController.onStopToUseObject = value; }

		//InteractabeController
		public static Action<ActionType, float> OnStartToInteract { get => instance.interactableController.onStartToInteract; set => instance.interactableController.onStartToInteract = value; }
		public static Action<ActionType, float> OnInteract { get => instance.interactableController.onInteract; set => instance.interactableController.onInteract = value; }
		public static Action<ActionType, float> OnStopToInteract { get => instance.interactableController.onStopToInteract; set => instance.interactableController.onStopToInteract = value; }

		public static int HowManyItem(string name)
		{
			return 0;
		}
		public static bool HasItemAmount(string name, int amount)
		{
			return false;
		}

		public static void ActiveControl()
		{
			instance.hasControl = true;
			instance.onActiveControl.Invoke();
		}
		public static void DesactiveControl()
		{
			instance.hasControl = false;
			instance.onDesactiveControl.Invoke();
		}


		CharacterController characterController = null;
		EquipableObjectController equipableObjectController = null;
		UsableObjectController usableObjectController = null;
		InteractableController interactableController = null;

		bool hasControl = true;
		Action onActiveControl = () => { /*Debug.Log("OnActiveControl");*/ };
		Action onDesactiveControl = () => { /*Debug.Log("OnDesactiveControl");*/ };

		private void Awake()
		{
			characterController = GetComponent<CharacterController>();
			equipableObjectController = GetComponent<EquipableObjectController>();
			usableObjectController = GetComponent<UsableObjectController>();
			interactableController = GetComponent<InteractableController>();

			instance = this;
		}
	}
}
