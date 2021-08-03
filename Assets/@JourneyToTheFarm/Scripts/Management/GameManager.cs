using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class GameManager : MonoBehaviour
	{
		static GameManager instance = null;

		public static DataBase DataBase => instance.dataBase;
		public static Camera playerCamera = null;
		public static Player player = null;

		public static void ActiveCursor()
		{
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		}
		public static void DesactiveCursor()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		[SerializeField] DataBase dataBase = null;

		private void Awake()
		{
			instance = this;
		}
		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
}
