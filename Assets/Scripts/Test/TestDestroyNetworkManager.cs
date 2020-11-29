using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.GameAdmin;
using Mirror;

namespace MyFPS.Test
{
    public class TestDestroyNetworkManager : MonoBehaviour
    {
        void Start()
        {
            if(NetworkManager.singleton)
            {
                Destroy(NetworkManager.singleton.gameObject);
            }
        }
    }
}