using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.GameAdmin;

namespace MyFPS.Player
{
    [RequireComponent(typeof(PlayerHandler))]
    public class PlayerRespawn : MonoBehaviour
    {
        /// <summary>
        /// Reference to the PlayerHandler class on this gameObject.
        /// </summary>
        PlayerHandler playerHandler;
        /// <summary>
        /// Reference to this game's GameMode.
        /// </summary>
        GameMode gameMode;
        /// <summary>
        /// Position to respawn the player to.
        /// </summary>
        Transform respawnPosition;
        void Start()
        {
            playerHandler = GetComponent<PlayerHandler>();
        }
    }
}