using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

#pragma warning disable IDE0044
#pragma warning disable IDE0051

namespace JTTF
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private DataBase dataBase = null;
		[SerializeField] private SeedDataBase seedDataBase = null;
		[SerializeField] private PrefabDataBase prefabDataBase = null;
		[SerializeField] private bool hideCursorOnPlay = true;

		private static GameManager instance = null;

		private static bool isMulti = false;

		public static Player player = null;
		public static TPCameraController cameraController = null;
		
		public static bool IsSolo => !isMulti;
		public static bool IsMulti => isMulti;
		public static DataBase DataBase => instance.dataBase;
		public static SeedDataBase SeedDataBase => instance.seedDataBase;
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

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.P))
			{
				if (cameraController.HasControl)
					cameraController.DesactiveControl();
				else
					cameraController.ActiveControl();
			}
		}
	}
}
