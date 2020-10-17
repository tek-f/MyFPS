using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFPS.Player
{
    public class Weapon : MonoBehaviour
    {
        public int teamID;
        public bool isWeaponLocked = false;
        public bool isWeaponDropable = false;
        public GameObject worldWeaponGameObject;
        public Vector3 originalLocation, equipedByPlayerPosition;
        public void SetUp(int teamID, GameObject worldGameObject, Vector3 originalLocation)
        {
            this.teamID = teamID;
            if (worldGameObject != null)
            {
                worldWeaponGameObject = worldGameObject;
            }
            this.originalLocation = originalLocation;
        }
        public void DropWeapon(CharacterController player)
        {
            //Get Location and Direction for drop
            Vector3 dropLocation = Vector3.zero;
            Ray rayToDropLocation = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(rayToDropLocation, out hit))
            {
                Debug.Log(hit.transform.name);
                dropLocation = hit.point;
            }


            //Drop Weapon
            worldWeaponGameObject.GetComponent<Animator>().enabled = false;//disable animator
            worldWeaponGameObject.transform.SetParent(null);//detach parent (player)
            worldWeaponGameObject.transform.rotation = Quaternion.Euler(0 ,0, 0);//reset rotation
            worldWeaponGameObject.transform.position = dropLocation;//move weapon to drop location

            //Use Renderer to ensure gun is not within the bounds of another object
            Renderer rend = worldWeaponGameObject.GetComponentInChildren<Renderer>();
            if (rend != null)
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
                worldWeaponGameObject.GetComponentInChildren<BoxCollider>().enabled = true;
                worldWeaponGameObject.transform.position = dropLocation;
            }
            else
            {
                Debug.LogWarning("Renderer for not found for drop weapon");
            }
        }
    }
}