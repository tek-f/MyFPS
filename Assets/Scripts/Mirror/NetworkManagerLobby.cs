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
        [Scene] [SerializeField] private string menuScene = string.Empty;
        [Scene] [SerializeField] public string levelScene = string.Empty;

        [Header("Room")]
        [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;

        [Header("Game")]
        [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
        [SerializeField] private GameObject playerSpawnSystem = null;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;

        public List<NetworkRoomPlayerLobby> roomPlayers { get; } = new List<NetworkRoomPlayerLobby>();//-------------
        public List<NetworkGamePlayerLobby> gamePlayers { get; } = new List<NetworkGamePlayerLobby>();

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
            if(SceneManager.GetActiveScene().path != menuScene)
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
            if (SceneManager.GetActiveScene().path == menuScene)
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
                player.HandleReadyToStart(IsReadyToStart());
            }
        }
        public bool IsReadyToStart()
        {
            if(numPlayers < minPlayers)
            {
                return true;
            }
            foreach (var player in roomPlayers)
            {
                if(!player.isReady)
                {
                    return false;
                }
            }
            Debug.LogWarning("IsReadyTooStart not returned");
            return true;
        }
        public void StartGame()
        {
            if(SceneManager.GetActiveScene().path == menuScene)
            {
                if(!IsReadyToStart())
                {
                    return;
                }
                if (levelScene != null)
                {
                    ServerChangeScene(levelScene);
                }
            }
        }
        public override void ServerChangeScene(string newSceneName)
        {
            if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Assets/Scenes/Scene_Map"))
            {
                for (int i = roomPlayers.Count - 1; i >= 0; i--)
                {
                    var conn = roomPlayers[i].connectionToClient;
                    var gamePlayersInstance = Instantiate(gamePlayerPrefab);
                    gamePlayersInstance.SetDisplayName(roomPlayers[i].displayName);

                    NetworkServer.Destroy(conn.identity.gameObject);
                    NetworkServer.ReplacePlayerForConnection(conn, gamePlayersInstance.gameObject, true);
                }
            }
            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if(sceneName.StartsWith("Assets/Scenes/Scene_Map"))
            {

                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);
            }
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            base.OnServerReady(conn);

            OnServerReadied?.Invoke(conn);
        }
    }
}