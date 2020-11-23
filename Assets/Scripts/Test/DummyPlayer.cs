using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.Player;

namespace MyFPS.Test
{
    public class DummyPlayer : MonoBehaviour
    {
        /// <summary>
        /// Health of the dummy player.
        /// </summary>
        public int health = 5;
        /// <summary>
        /// Tracks if the dummy player is alive/dead.
        /// </summary>
        bool dead = false;
        /// <summary>
        /// Reference to the Renderer on this game object.
        /// </summary>
        Renderer renderer;
        /// <summary>
        /// The delay between death and respawn.
        /// </summary>
        float respawnDelay;
        /// <summary>
        /// The time stamp of the most recent death.
        /// </summary>
        float deathTimeStamp;
        GameAdmin.GameModeDM gameManager;
        /// <summary>
        /// Death function.
        /// </summary>
        void Die()
        {
            deathTimeStamp = Time.time;
            renderer.enabled = false;
            //gameManager.AddScore(0, 1);
            dead = true;
        }
        /// <summary>
        /// Respawn function.
        /// </summary>
        void Respawn()
        {
            renderer.enabled = true;
            health = 5;
            dead = false;
        }
        private void Start()
        {
            renderer = GetComponent<MeshRenderer>();
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameAdmin.GameModeDM>();
            respawnDelay = 5f;
        }
        private void Update()
        {
            if (!dead)
            {
                if (health <= 0)
                {
                    Die();
                }
            }
            else if (Time.time - deathTimeStamp >= respawnDelay)
            {
                Respawn();
            }
        }
    }
}