﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.Player;

namespace MyFPS.GameAdmin
{
    public class CaptureZone : MonoBehaviour
    {
        [SerializeField] int teamID;

        GameModeCTF gameModeCTF;
        [SerializeField] GameObject worldSpaceFlag;

        private void Start()
        {
            gameModeCTF = FindObjectOfType<GameModeCTF>();

            if (gameModeCTF == null)
            {
                Debug.LogError("Could not fund GameModeCTF");
            }
        }
        private void ResetFlag()
        {
            worldSpaceFlag.transform.position = worldSpaceFlag.GetComponent<Flag>().originalLocation;
            worldSpaceFlag.SetActive(true);
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
                    gameModeCTF.AddScore(player.teamID, 1);
                    player.ReturnFlag();
                    ResetFlag();
                }
            }
        }
    }
}