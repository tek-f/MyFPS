﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.Player;
using Mirror;

namespace MyFPS.GameAdmin
{
    public class CaptureZone : NetworkBehaviour
    {
        /// <summary>
        /// Team ID of the capture zone.
        /// </summary>
        [SerializeField] int teamID;
        /// <summary>
        /// Reference to the Game Manager.
        /// </summary>
        [SerializeField] GameModeCTF gameModeCTF;
        /// <summary>
        /// Reference to this capture zones game object that is in the world for players to pick up.
        /// </summary>
        [SerializeField] GameObject worldSpaceFlag;
        /// <summary>
        /// Sets worldSpaceFlag position to it's originalPosition.
        /// </summary>
        private void ResetFlag()
        {
            worldSpaceFlag.transform.position = worldSpaceFlag.GetComponent<Flag>().originalLocation;
            worldSpaceFlag.SetActive(true);
        }
        [Command]
        void CmdAddScore()
        {

        }

        private void Start()
        {
            //gameModeCTF = GameObject.FindWithTag("GameManager").GetComponent<GameModeCTF>();

            if (gameModeCTF == null)
            {
                Debug.LogError("Could not fund GameModeCTF");
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            PlayerHandler player = other.GetComponent<PlayerHandler>();

            if (player != null && gameModeCTF != null)
            {
                if (player.GetWeaponTeamID() != teamID)
                {
                    return;
                }

                if (player.IsHoldingFlag)
                {
                    gameModeCTF.CmdUpdateScoreNetwork(player.teamID);
                    player.ReturnFlag();
                    ResetFlag();
                }
            }
        }
    }
}