using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

namespace MyFPS.Mirror
{
    public class NetworkRoomPlayerLobby : NetworkBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject lobbyUI;
        [SerializeField] private TMP_Text[] playerNameTexts = new TMP_Text[4];
        [SerializeField] private TMP_Text[] playerReadyTexts = new TMP_Text[4];
        [SerializeField] private Button startGameButton;

        [SyncVar(hook = nameof(HandleDisplayNameChange))]
        public string displayName = "Loading...";
        [SyncVar(hook = nameof(HandleReadyStatusChange))]
        public bool isReady = false;

        private bool isLeader;
        public bool IsLeader
        {
            set
            {
                value = isLeader;
                startGameButton.gameObject.SetActive(value);
            }
        }

        private NetworkManagerLobby room;
        private NetworkManagerLobby Room
        {
            get
            {
                if(room != null)
                {
                    return room;
                }

                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public override void OnStartAuthority()
        {
            //CMDSetDisplayName(PlayerNameInput.DisplayName);

            lobbyUI.SetActive(true);
        }
        public override void OnStartClient()
        {
            Room.roomPlayers.Add(this);
            UpdateDisplay();
        }

        public override void OnStopClient()
        {
            Room.roomPlayers.Remove(this);
            UpdateDisplay();
        }
        public void HandleReadyStatusChange(bool oldValue, bool newValue) => UpdateDisplay();
        public void HandleDisplayNameChange(string oldValue, string newValue) => UpdateDisplay();

        private void UpdateDisplay()
        {
            if(!isLocalPlayer)
            {
                foreach (var player in Room.roomPlayers)
                {
                    if(player.isLocalPlayer)
                    {
                        player.UpdateDisplay();
                        break;
                    }
                }
                return;
            }
            for (int i = 0; i < playerNameTexts.Length; i++)
            {
                playerNameTexts[i].text = "Waiting for player...";
                playerReadyTexts[i].text = string.Empty;
            }
        }
    }
}