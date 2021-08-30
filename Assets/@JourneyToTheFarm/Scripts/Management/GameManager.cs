using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class GameManager : MonoBehaviour
	{
		static GameManager instance = null;

		public static DataBase DataBase => instance.dataBase;
		public static PrefabDataBase PrefabDataBase => instance.prefabDataBase;
		public static Camera playerCamera = null;
		public static Player player = null;

		[SerializeField] DataBase dataBase = null;
		[SerializeField] PrefabDataBase prefabDataBase = null;
		[SerializeField] private bool hideCursorOnPlay = true;

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

		private void Awake()
		{
			instance = this;
		}
		private void Start()
		{
			if (hideCursorOnPlay)
				DesactiveCursor();
		}
	}
}
