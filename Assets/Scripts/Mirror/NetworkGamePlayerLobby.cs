using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MyFPS.Mirror
{
    public class NetworkGamePlayerLobby : NetworkBehaviour
    {
        [SyncVar] private string displayName = "Loading...";

        private NetworkManagerLobby room;
        private NetworkManagerLobby Room
        {
            get
            {
                if(room != null)
                {
                    return room;
                }
                room = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManagerLobby>();
                return room;
            }
        }

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);

            Room.gamePlayers.Add(this);
        }
        public override void OnStopClient()
        {
            Room.gamePlayers.Remove(this);
        }
        [Server]
        public void SetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }
    }
}