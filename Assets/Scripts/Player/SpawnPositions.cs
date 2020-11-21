using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.Player;

namespace MyFPS.GameAdmin
{
    public class SpawnPositions : MonoBehaviour
    {
        public Transform[] team1SpawnPoints;
        public Transform[] team2SpawnPoints;
        [SerializeField] GameObject team1SpawnPointsParent, team2SpawnPointsParent;

        private void Awake()
        {
            team1SpawnPoints = team1SpawnPointsParent.GetComponentsInChildren<Transform>();
            team2SpawnPoints = team2SpawnPointsParent.GetComponentsInChildren<Transform>();
        }
    }
}