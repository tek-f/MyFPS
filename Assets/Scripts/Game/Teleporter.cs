using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.Player;

namespace MyFPS.GameAdmin
{
    public class Teleporter : MonoBehaviour
    {
        /// <summary>
        /// Transform of an object that marks the exit point of the teleporter. Is used in Teleport.
        /// </summary>
        public Transform teleportPoint;
        /// <summary>
        /// Teleports _player to the position of teleportPoint.
        /// </summary>
        /// <param name="_player">Transform of the player that is being teleported</param>
        public void Teleport(Transform _player)
        {
            print("teleport entered");
            _player.position = teleportPoint.position;
            print("teleported?");
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Teleport(other.transform);
            }
        }
    }
}