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
        [SerializeField] private int minPlayers = 2;//-------------
        [Scene] [SerializeField] private string MenuScene = string.Empty;
        List<List<GameObject>> testlist = new List<List<GameObject>>();

        [Header("Room")]
        [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;

        public List<NetworkRoomPlayerLobby> roomPlayers { get; } = new List<NetworkRoomPlayerLobby>();//-------------

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
            if(SceneManager.GetActiveScene().path != MenuScene)
            {
                NetworkRoomPlayerLobby roomPlayerLobby = Instantiate(roomPlayerPrefab);
            }
        }
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if(conn.identity)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

                roomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);
        }
        public override void OnServerAddPlayer(NetworkConnection networkConnection)
        {
            if (SceneManager.GetActiveScene().path == MenuScene)
            {
                bool isLeader = roomPlayers.Count == 0;

                NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

                roomPlayerInstance.IsLeader = isLeader;

                NetworkServer.AddPlayerForConnection(networkConnection, roomPlayerInstance.gameObject);
            }
        }
        public override void OnStopServer()
        {
            roomPlayers.Clear();
        }
        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in roomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart);
            }
        }
        public bool IsReadyToStart()
        {
            if(numPlayers < minPlayers)
            {
                return;
            }
            foreach (var player in roomPlayers)
            {
                if(!player.isReady)
                {
                    return false;
                }
            }
        }
    }
}