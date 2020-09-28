using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MyFPS.Mirror
{
    public class MirrorPlayerHandler : NetworkBehaviour
    {
        [SerializeField] private Vector3 movement = new Vector3();
        [Client]
        private void Update()
        {
            if (!hasAuthority)
            {
                return;
            }
            if (Input.GetButtonDown("Jump"))
            {
                CmdMove();
            }
        }
        [Command]
        private void CmdMove()
        {
            RpcMove();
        }
        [ClientRpc]
        private void RpcMove() => transform.Translate(movement);
    }
}