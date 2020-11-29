using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFPS.Player
{
    public class Weapon : MonoBehaviour
    {
        /// <summary>
        /// Team ID of the weapon.
        /// </summary>
        public int teamID;
        /// <summary>
        /// Determines if the weapon allows the player to change weapons while it is equiped. Used for the flag.
        /// </summary>
        public bool isWeaponLocked = false;
        /// <summary>
        /// Determines if the weapon is dropable.
        /// </summary>
        public bool isWeaponDropable = false;
        /// <summary>
        /// World space object of the weapon
        /// </summary>
        public GameObject worldWeaponGameObject;
        /// <summary>
        /// Weapons original locaiton.
        /// </summary>
        public Vector3 originalLocation;
        /// <summary>
        /// Location the weapon was first equiped by the player.
        /// </summary>
        public Vector3 equipedByPlayerPosition;
        /// <summary>
        /// Set Up function for the weapon.
        /// </summary>
        /// <param name="_teamID">Value that teamID is set to.</param>
        /// <param name="_worldGameObject">Value that worldWeaponGameObject is set to.</param>
        /// <param name="_originalLocation">Value that originalLocation is set to.</param>
        public void SetUp(int _teamID, GameObject _worldGameObject, Vector3 _originalLocation)
        {
            this.teamID = _teamID;
            if (_worldGameObject != null)
            {
                worldWeaponGameObject = _worldGameObject;
            }
            this.originalLocation = _originalLocation;
        }
        /// <summary>
        /// Drops this weapon from the players loadout.
        /// </summary>
        /// <param name="player">Player that is dropping this weapon.</param>
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