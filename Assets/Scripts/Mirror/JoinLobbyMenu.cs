using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MyFPS.Mirror
{
    public class JoinLobbyMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager;

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel;
        [SerializeField] private TMP_InputField ipAddressInputField;
        [SerializeField] private Button joinButton;

        private void OnEnable()
        {
            NetworkManagerLobby.OnClientConnected += HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
        }
        private void OnDisable()
        {
            NetworkManagerLobby.OnClientConnected += HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
        }

        public void JoinLobby()
        {
            networkManager.networkAddress = ipAddressInputField.text;
            networkManager.StartClient();

            joinButton.interactable = false;
        }
        private void HandleClientConnected()
        {
            joinButton.interactable = true;

            landingPagePanel.SetActive(false);
            gameObject.SetActive(false);
        }
        private void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }
    }
}