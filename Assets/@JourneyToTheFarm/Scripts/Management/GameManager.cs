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

		[SerializeField] DataBase dataBase = null;

		int openedPanelCount = 0;

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
