using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFPS.Mirror
{
    public class MainMenu : MonoBehaviour
    {

        [SerializeField] private NetworkManagerLobby networkManager;

        [Header("IU")]
        [SerializeField] private GameObject landingPagePanel;
        public void HostLobby()
        {
            networkManager.StartHost();
            landingPagePanel.SetActive(false);
        }
    }
}