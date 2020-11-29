using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFPS.Player;

public class Flag : MonoBehaviour
{
    /// <summary>
    /// Team ID of the flag.
    /// </summary>
    [SerializeField] int teamID;
    /// <summary>
    /// Location the flag resets itself to after it has been returned to it's CaptureZone. 
    /// </summary>
    public Vector3 originalLocation;
    /// <summary>
    /// weaponID of the flag in the PlayerHandler script.
    /// </summary>
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

            player.PickUpFlag();

            gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        originalLocation = transform.position;
    }
}
