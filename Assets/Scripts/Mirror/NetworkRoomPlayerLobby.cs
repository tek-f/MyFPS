using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine;

namespace MyFPS.Mirror
{
    public class NetworkRoomPlayerLobby : NetworkManager
    {
        [Scene] [SerializeField] private string MenuScene = string.Empty;
        List<List<GameObject>> testlist = new List<List<GameObject>>();

        [Header("Room")]
        [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;

        public override void OnStartServer()
        {
            spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
        }

        public override void OnStartClient()
        {
            //base.OnStartClient();

            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {

            }
        }
    }
}