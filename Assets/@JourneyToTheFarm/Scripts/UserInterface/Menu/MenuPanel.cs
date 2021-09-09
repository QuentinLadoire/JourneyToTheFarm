using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MLAPI;
using MLAPI.SceneManagement;

#pragma warning disable IDE0044
#pragma warning disable IDE0051

namespace JTTF
{
    public class MenuPanel : MonoBehaviour
    {
		[SerializeField] private CanvasGroupBehaviour playmodeButtonContainer = null;
		[SerializeField] private CanvasGroupBehaviour multiplayerButtonContainer = null;
		[SerializeField] private CanvasGroupBehaviour wantToQuitPopup = null;

		private void Start()
		{
			NetworkManager.Singleton.OnServerStarted += OnServerStarted;
		}
		private void OnDestroy()
		{
			NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
		}

		private void OnServerStarted()
		{
			NetworkSceneManager.SwitchScene("Lobby");
		}

		public void OnSoloButtonClicked()
		{
			GameManager.PlaySolo();
			SceneManager.LoadScene("MapGeneration");
		}
        public void OnMultiplayerButtonClicked()
		{
			playmodeButtonContainer.SetActive(false);
			multiplayerButtonContainer.SetActive(true);
		}
		public void OnReturnToDesktopButtonClicked()
		{
			playmodeButtonContainer.CanvasGroup.interactable = false;
			wantToQuitPopup.SetActive(true);
		}
		
		public void OnYesButtonClicked()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.ExitPlaymode();
#else
			Application.Quit();
#endif
		}
		public void OnNoButtonClicked()
		{
			wantToQuitPopup.SetActive(false);
			playmodeButtonContainer.CanvasGroup.interactable = true;
		}

		public void OnHostButtonClicked()
		{
			NetworkManager.Singleton.StartHost();
		}
		public void OnJoinButtonClicked()
		{
			NetworkManager.Singleton.StartClient();
		}
		public void OnCancelButtonClicked()
		{
			multiplayerButtonContainer.SetActive(false);
			playmodeButtonContainer.SetActive(true);
		}
    }
}
