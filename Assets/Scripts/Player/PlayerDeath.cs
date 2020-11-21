using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFPS.Player
{
    public class PlayerDeath : MonoBehaviour
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
            transform.position = playerHandler.respawnPosition.position;

            playerHandler.enabled = true;
            fpsController.enabled = true;
            deathPanel.SetActive(false);

            this.enabled = false;
        }
        private void Awake()
        {
            playerHandler = gameObject.GetComponent<PlayerHandler>();
            fpsController = gameObject.GetComponent<FirstPersonController>();
        }
        private void OnEnable()
        {
            playerHandler.enabled = false;
            fpsController.enabled = false;
            deathPanel.SetActive(true);

            deathTimeStamp = Time.time;

            respawnCounterText.text = respawnDelay.ToString();
        }
        private void Update()
        {
            float timeRemaining = respawnDelay;
            timeRemaining -= (Time.time - deathTimeStamp);
            respawnCounterText.text = timeRemaining.ToString();
            if (timeRemaining <= 0)
            {
                Respawn();
            }
        }
    }
}