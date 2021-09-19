using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JTTF.Behaviour;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.SceneManagement;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;

#pragma warning disable IDE0044
#pragma warning disable IDE0090

namespace JTTF.Lobby
{
    public class LobbyPanel : CustomNetworkBehaviour
    {
		[Header("Lobby Setting")]
		[SerializeField] private PlayerCard[] playerCards = null;
		[SerializeField] private Button startButton = null;

        NetworkList<LobbyPlayerState> lobbyPlayerStates = new NetworkList<LobbyPlayerState>(new NetworkVariableSettings
		{
			WritePermission = NetworkVariablePermission.ServerOnly,
			ReadPermission = NetworkVariablePermission.Everyone			
		});

		private void OnLobbyPlayerStateListChanged(NetworkListEvent<LobbyPlayerState> changeEvent)
		{
			for (int i = 0; i < playerCards.Length; i++)
			{
				if (i < lobbyPlayerStates.Count)
					playerCards[i].UpdateDisplay(lobbyPlayerStates[i]);
				else
					playerCards[i].DisableDisplay();
			}

			if (IsHost)
			{
				startButton.interactable = IsAllReady();
			}
		}
		private void OnClientConnectedCallback(ulong clientId)
		{
			int index = NetworkManager.Singleton.ConnectedClientsList.Count;
			lobbyPlayerStates.Add(new LobbyPlayerState(clientId, "Player " + index, false));
		}
		private void OnClientDisconnectCallback(ulong clientId)
		{
			for (int i = 0; i < lobbyPlayerStates.Count; i++)
			{
				if (lobbyPlayerStates[i].clientId == clientId)
				{
					lobbyPlayerStates.RemoveAt(i);
					break;
				}
			}
		}

		private bool IsAllReady()
		{
			foreach (var playerState in lobbyPlayerStates)
				if (!playerState.isReady)
					return false;

			return true;
		}

		[ServerRpc(RequireOwnership = false)]
		private void ToggleReadyServerRpc(ulong clientId)
		{
			for (int i = 0; i < lobbyPlayerStates.Count; i++)
			{
				if (lobbyPlayerStates[i].clientId == clientId)
					lobbyPlayerStates[i] = new LobbyPlayerState
					{ 
						clientId = lobbyPlayerStates[i].clientId,
						playerName = lobbyPlayerStates[i].playerName,
						isReady = !lobbyPlayerStates[i].isReady 
					};
			}
		}

		public override void NetworkStart()
		{
			base.NetworkStart();

			//Register the client callback to update the LobbyPlayerCard UI
			if (IsClient)
			{
				lobbyPlayerStates.OnListChanged += OnLobbyPlayerStateListChanged;
			}

			//For each connected client, add it the the LobbyPlayerState List
			if (IsServer)
			{
				NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
				NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

				int index = 0;
				foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
				{
					lobbyPlayerStates.Add(new LobbyPlayerState(client.ClientId, "Player " + (index + 1), false));

					index++;
				}

				startButton.gameObject.SetActive(true);
				startButton.interactable = false;
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			//Unregister all callback

			lobbyPlayerStates.OnListChanged -= OnLobbyPlayerStateListChanged;

			if (NetworkManager.Singleton != null)
			{
				NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
				NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
			}
		}

		public void OnLeaveButtonClicked()
		{
			if (IsHost)
			{
				NetworkManager.Singleton.StopHost();
			}
			else if (IsClient)
			{
				NetworkManager.Singleton.StopClient();
			}

			SceneManager.LoadScene("MainMenu");
		}
		public void OnReadyButtonClicked()
		{
			ToggleReadyServerRpc(NetworkManager.Singleton.LocalClientId);
		}
		public void OnStartButtonClicked()
		{
			NetworkSceneManager.SwitchScene("MapGeneration");
		}
	}
}
