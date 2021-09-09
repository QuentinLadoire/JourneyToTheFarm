using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JTTF.Lobby
{
    public class PlayerCard : UIBehaviour
    {
        [SerializeField] private GameObject waitingForPlayer = null;
        [SerializeField] private GameObject connectedPlayerInfo = null;
        [SerializeField] private Text playerNameText = null;
        [SerializeField] private Toggle isReadyToggle = null;
        
        public void UpdateDisplay(LobbyPlayerState lobbyPlayerState)
		{
            playerNameText.text = lobbyPlayerState.playerName;
            isReadyToggle.isOn = lobbyPlayerState.isReady;

            waitingForPlayer.SetActive(false);
            connectedPlayerInfo.SetActive(true);
		}
        public void DisableDisplay()
		{
            waitingForPlayer.SetActive(true);
            connectedPlayerInfo.SetActive(false);
		}
    }
}
