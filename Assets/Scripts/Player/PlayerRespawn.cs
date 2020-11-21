using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.GameAdmin;

namespace MyFPS.Player
{
    [RequireComponent(typeof(PlayerHandler))]
    public class PlayerRespawn : MonoBehaviour
    {
        PlayerHandler playerHandler;
        GameMode gameMode;
        Transform respawnPosition;
        void Start()
        {
            playerHandler = GetComponent<PlayerHandler>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}