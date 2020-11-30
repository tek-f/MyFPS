using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;
using MyFPS.Player;

namespace MyFPS.Mirror
{
    public class PlayerSpawnSystem : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;

        private static List<Transform> spawnPoints = new List<Transform>();

        [SyncVar] int index = 0;

        public static void AddSpawnPoint(Transform transform)
        {
            spawnPoints.Add(transform);
            spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        }

        public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

        public override void OnStartServer() => NetworkManagerLobby.OnServerReadied += SpawnPlayer;

        [Command]
        public void CmdSetPlayerRespawnPos(GameObject _player, Transform _respawnPos)
        {
            //_player.GetComponent<PlayerHandler>().RpcSetRespawnPos(_respawnPos);
        }

        [ServerCallback]
        private void OnDestroy() => NetworkManagerLobby.OnServerReadied -= SpawnPlayer;
        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            if (index >= 2)
            {
                index = 0;
            }

            Transform spawnPoint = spawnPoints[index];

            if (spawnPoint == null)
            {
                Debug.LogError("Spawn point transform not found");
                return;
            }

            GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            //CmdSetPlayerRespawnPos(playerInstance, spawnPoint);
            playerInstance.GetComponent<PlayerHandler>().teamID = index;
            NetworkServer.Spawn(playerInstance, conn);

            index++;
        }
    }
}