using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.Player;

namespace MyFPS.GameAdmin
{
    public class SpawnPositions : MonoBehaviour
    {
        /// <summary>
        /// Array of transforms that represent team 1 spawn points.
        /// </summary>
        public Transform[] team1SpawnPoints;
        /// <summary>
        /// Array of transforms that represent team 2 spawn points.
        /// </summary>
        public Transform[] team2SpawnPoints;
        /// <summary>
        /// Parent of team 1 spawn points.
        /// </summary>
        [SerializeField] GameObject team1SpawnPointsParent;
        /// <summary>
        /// Parent of team 2 spawn points.
        /// </summary>
        [SerializeField] GameObject team2SpawnPointsParent;
        private void Awake()
        {
            team1SpawnPoints = team1SpawnPointsParent.GetComponentsInChildren<Transform>();
            team2SpawnPoints = team2SpawnPointsParent.GetComponentsInChildren<Transform>();
        }
    }
}