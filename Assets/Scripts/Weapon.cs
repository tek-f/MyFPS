using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int teamID;
    public bool isWeaponLocked = false;
    public bool isWeaponDropable = false;

    public GameObject worldWeaponGameObject;
    public Vector3 originalLocation;


    public void SetUp(int teamID, GameObject worldGameObject, Vector3 originalLocation)
    {
        this.teamID = teamID;
        if(worldGameObject != null)
        {
            worldWeaponGameObject = worldGameObject;
        }

        this.originalLocation = originalLocation;
    }
    public void DropWeapon(CharacterController player, Vector3 dropLocation)
    {
        Vector3 dropDirection = dropLocation - Camera.main.transform.position;

        Ray rayToDropLocation = new Ray(Camera.main.transform.position, dropDirection);
        RaycastHit hit;

        if(Physics.Raycast(rayToDropLocation, out hit, dropDirection.magnitude))
        {
            dropLocation = hit.point;
        }

        worldWeaponGameObject.transform.position = dropLocation;

        Renderer rend = worldWeaponGameObject.GetComponent<Renderer>();
        if(rend != null)
        {
            Debug.Log("Dropping using renderer: " + rend.name);

            Vector3 topPoint = rend.bounds.center;
            topPoint.y += rend.bounds.extents.y;

            float height = rend.bounds.extents.y * 2;

            Ray rayDown = new Ray(topPoint, Vector3.down);
            RaycastHit rayDownHit;
            if (Physics.Raycast(rayDown, out rayDownHit, height * 1.05f))
            {
                dropLocation = rayDownHit.point;
                dropLocation.y += (rend.bounds.extents.y * 1.1f);
            }

            worldWeaponGameObject.transform.position = dropLocation;
        }
        else
        {
            Debug.LogError("Renderer for not found for drop weapon");
        }

        Rigidbody weaponRgBdy = worldWeaponGameObject.GetComponent<Rigidbody>();

        if(weaponRgBdy != null && player != null)
        {
            weaponRgBdy.velocity = player.velocity;
        }
    }
    private void Start()
    {

    }
}
