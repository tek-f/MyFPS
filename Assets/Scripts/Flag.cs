using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] int teamID;
    public Vector3 originalLocation;

    const int weaponID = 1;
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if(player != null)
        {
            if(player.teamID != teamID)
            {
                return;
            }

            Debug.Log("Flag captured");

            player.PickUpWeapon(gameObject, originalLocation, teamID, weaponID);

            gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        originalLocation = transform.position;
    }
}
