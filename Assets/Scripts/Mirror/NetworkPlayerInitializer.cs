using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MyFPS.Player;
using UnityEngine.InputSystem;
using MyFPS.GameAdmin;


namespace MyFPS.Mirror
{
    public class NetworkPlayerInitializer : NetworkBehaviour
    {
        [SerializeField] Camera playerCamera;
        [SerializeField] Canvas playerCanvas;
        PlayerHandler playerHandler;
        FirstPersonController fpsController;
        PlayerInput playerInput;
        public override void OnStartAuthority()
        {
            playerHandler = GetComponent<PlayerHandler>();
            playerHandler.enabled = true;
            playerInput = GetComponent<PlayerInput>();
            playerInput.enabled = true;
            fpsController = GetComponent<FirstPersonController>();
            fpsController.enabled = true;
            playerCamera = gameObject.GetComponentInChildren<Camera>();
            playerCamera.enabled = true;
            playerCanvas = gameObject.GetComponentInChildren<Canvas>();
            playerCanvas.enabled = true;

            GameMode gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameMode>();
            gameManager.playersList.Add(playerHandler);
            gameManager.localPlayer = playerHandler;
        }
    }
}