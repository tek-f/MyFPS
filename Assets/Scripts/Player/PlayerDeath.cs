using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Mirror;

namespace MyFPS.Player
{
    public class PlayerDeath : NetworkBehaviour
    {
        /// <summary>
        /// Reference to the PlayerHandler class on this game object.
        /// </summary>
        PlayerHandler playerHandler;
        /// <summary>
        /// Reference to the FirstPersonController class on this game object.
        /// </summary>
        FirstPersonController fpsController;
        /// <summary>
        /// Reference to the PlayerInput on this game object.
        /// </summary>
        PlayerInput playerInput;
        /// <summary>
        /// Reference to the UI panel displayed on the players death.
        /// </summary>
        public GameObject deathPanel;
        /// <summary>
        /// Reference to the UI Text that counts down the player respawn.
        /// </summary>
        public Text respawnCounterText;
        /// <summary>
        /// Time stamp of the players most recent death. Used in Update().
        /// </summary>
        float deathTimeStamp;
        /// <summary>
        /// The delay after players death until the respawn.
        /// </summary>
        float respawnDelay = 8f;
        /// <summary>
        /// Respawns the player.
        /// </summary>
        void Respawn()
        {
            playerHandler.health = playerHandler.maxHealth;
            transform.position = playerHandler.respawnPosition;
            
            playerInput.enabled = true;
            playerHandler.enabled = true;
            fpsController.enabled = true;
            deathPanel.SetActive(false);

            this.enabled = false;
        }
        [ClientRpc]
        public void RpcRespawn()
        {
            Respawn();
        }
        [Command]
        public void CmdRespawn()
        {
            RpcRespawn();
        }
        private void Awake()
        {
            playerHandler = gameObject.GetComponent<PlayerHandler>();
            fpsController = gameObject.GetComponent<FirstPersonController>();
            playerInput = gameObject.GetComponent<PlayerInput>();
        }
        private void OnEnable()
        {
            fpsController = gameObject.GetComponent<FirstPersonController>();
            playerHandler.enabled = false;
            fpsController.enabled = false;
            playerInput.enabled = false;
            deathPanel.SetActive(true);

            deathTimeStamp = Time.time;

            respawnCounterText.text = respawnDelay.ToString();
        }
        private void Update()
        {
            float remaining = Mathf.Clamp(respawnDelay - (Time.time - deathTimeStamp), 0, respawnDelay);
            respawnCounterText.text = remaining.ToString();
            if (remaining <= 0)
            {
                CmdRespawn();
            }
        }
    }
}