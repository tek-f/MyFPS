using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MyFPS.Player;


namespace MyFPS.Mirror
{
    public class NetworkPlayerInitializer : NetworkBehaviour
    {
        [SerializeField] Camera playerCamera;
        [SerializeField] GameObject playerCanvas;
        public override void OnStartAuthority()
        {
            if (isLocalPlayer)
            {
                GetComponent<PlayerHandler>().enabled = true;
                GetComponent<FirstPersonController>().enabled = true;
                GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
                playerCamera.enabled = true;
                playerCanvas.SetActive(true);

            }
        }
    }
}