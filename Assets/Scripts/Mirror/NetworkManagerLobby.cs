using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

namespace MyFPS.Mirror
{
    public class NetworkManagerLobby : NetworkManager
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
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {
                ClientScene.RegisterPrefab(prefab);
            }
        }

        public override void OnClientDisconnect(NetworkConnection networkConnection)
        {
            base.OnClientDisconnect(networkConnection);

            OnClientDisconnected?.Invoke();
        }

        public override void OnServerConnect(NetworkConnection networkConnection)
        {
            if(numPlayers >= maxConnections)
            {
                networkConnection.Disconnect();
                return;
            }
            if(SceneManager.GetActiveScene().name != MenuScene)
            {
                NetworkRoomPlayerLobby roomPlayerLobby = Instantiate(roomPlayerPrefab);
            }
        }
        public override void OnServerAddPlayer(NetworkConnection networkConnection)
        {
            if (SceneManager.GetActiveScene().name == MenuScene)
            {
                NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

                NetworkServer.AddPlayerForConnection(networkConnection, roomPlayerInstance.gameObject);
            }
        }
    }
}