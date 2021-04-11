using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JTTF
{
	public class GameManager : MonoBehaviour
	{
		static GameManager instance = null;

		public static DataBase DataBase { get => instance.dataBase; }

		[SerializeField] DataBase dataBase = null;

		private void Awake()
		{
			instance = this;
		}
	}
}
