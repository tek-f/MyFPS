using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFPS.Player
{
    public class PlayerHandler : MonoBehaviour
    {
        #region Player Metrics
        public int health = 3;
        public bool alive;
        #endregion
        #region Game Mode
        [SerializeField] int playersTeamID;
        public int teamID { get { return playersTeamID; } }
        CharacterController charConrtol;
        #endregion

        #region Weapons
        public List<Weapon> weapons;
        int currentWeapon = 0, lastWeapon = 0;
        Gun currentGun;
        public Vector3 dropOffset;

        #endregion
        private void SwitchWeapon(int weaponID, bool overrideLock = false)
        {
            if (weapons[weaponID] != null && !overrideLock && weapons[currentWeapon].isWeaponLocked == true)
            {
                return;
            }
            lastWeapon = currentWeapon;
            currentWeapon = weaponID;

            if(weapons[lastWeapon] != null)
            {
                weapons[lastWeapon].gameObject.SetActive(false);
            }
            weapons[currentWeapon].gameObject.SetActive(true);
            currentGun = weapons[currentWeapon].GetComponent<Gun>();
            currentGun.OnWeaponSwap();
        }

        public void PickUpWeapon(GameObject weaponObject, Vector3 originalLocation, int teamID, int weaponID, bool overrideLock = false)
        {
            if(weapons[currentWeapon] != null)
            {  
                
            }

            SwitchWeapon(weaponID, overrideLock);

            weapons[weaponID].SetUp(teamID, weaponObject, originalLocation);
        }
        public void DropWeapon(int weaponID)
        {
            if (weapons[weaponID].isWeaponDropable)
            {
                Vector3 forward = transform.forward;
                forward *= dropOffset.x;
                forward *= dropOffset.y;

                Vector3 dropLocation = transform.position + forward;

                weapons[weaponID].DropWeapon(charConrtol);
                
                weapons[currentWeapon] = null;
                SwitchWeapon(lastWeapon, true);
            }
        }
        public void ReturnWeapon(int weaponID)
        {
            if (weapons[weaponID].isWeaponDropable)
            {
                Vector3 returnLocation = weapons[weaponID].originalLocation;

                weapons[weaponID].worldWeaponGameObject.transform.position = returnLocation;
                weapons[weaponID].worldWeaponGameObject.SetActive(true);

                SwitchWeapon(lastWeapon, true);
            }
        }

        public int GetWeaponTeamID()
        {
            return weapons[currentWeapon].teamID;
        }

        public bool IsHolding(int weaponID)
        {
            if (currentWeapon == weaponID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Death();
            }
        }
        public void Death()
        {
            alive = false;

            //To be used when rigid bodies on players has been set up. Is not usable for now. 21/09/2020
            //gameObject.GetComponentInChildren<RagdollController>().Death();
        }

        private void Start()
        {
            //Initial Weapon Set Up
            foreach (Weapon weapon in weapons)
            {
                weapon.SetUp(teamID, weapon.gameObject, Vector3.zero);
                weapon.gameObject.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            charConrtol = gameObject.GetComponent<CharacterController>();
            SwitchWeapon(currentWeapon);
            lastWeapon = 1;
        }
        private void Update()
        {
            //Testing
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropWeapon(currentWeapon);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(1);
            }
            if(Input.GetKeyDown(KeyCode.C) && !currentGun.reloading)
            {
                SwitchWeapon(lastWeapon);
            }
        }
    }
}