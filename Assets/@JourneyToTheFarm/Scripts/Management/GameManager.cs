using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

namespace JTTF
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private DataBase dataBase = null;
		[SerializeField] private PrefabDataBase prefabDataBase = null;
		[SerializeField] private bool hideCursorOnPlay = true;

		private static GameManager instance = null;

		private static bool isMulti = false;

		public static Camera playerCamera = null;
		public static Player player = null;
		
		public static bool IsSolo => !isMulti;
		public static bool IsMulti => isMulti;
		public static DataBase DataBase => instance.dataBase;
		public static PrefabDataBase PrefabDataBase => instance.prefabDataBase;

		public static void PlaySolo()
		{
			isMulti = false;
		}
		public static void PlayMultiplayer()
		{
			isMulti = true;
		}

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
			if (instance != null)
			{
				Destroy(gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		private void Start()
		{
			if (hideCursorOnPlay)
				DesactiveCursor();
		}
	}
}
