using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFPS.Mirror
{
    public class MainMenu : MonoBehaviour
    {

        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel;
        public void HostLobby()
        {
            NetworkManagerLobby.instance.StartHost();
            landingPagePanel.SetActive(false);
        }
    }
}