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
        [SerializeField] private TMP_InputField isAddressInputField;
        [SerializeField] private Button joinButton;
    }
}