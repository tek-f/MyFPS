using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.Player;

public class Flag : MonoBehaviour
{
    [SerializeField] int teamID;
    public Vector3 originalLocation;

    const int weaponID = 1;
    private void OnTriggerEnter(Collider other)
    {
        PlayerHandler player = other.GetComponent<PlayerHandler>();

        if(player != null)
        {
            if(player.teamID != teamID)
            {
                return;
            }

            Debug.Log("Flag captured");

            player.PickUpFlag();

            gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        originalLocation = transform.position;
    }
}
