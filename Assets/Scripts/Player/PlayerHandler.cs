using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
        #region Input
        [SerializeField] PlayerInput playerInput;
        InputAction reloadAction;
        InputAction fireAction;
        #endregion
        #region Weapons
        public List<Weapon> weapons;
        [SerializeField] GameObject flagObject;
        int currentWeapon = 0, lastWeapon = 0;
        Gun currentGun;
        public Vector3 dropOffset, equippedWeaponPosition;
        public bool IsHoldingFlag
        {
            get
            {
                return flagObject.activeSelf;
            }
        }

        #endregion
        #region HUD
        [SerializeField] GameObject infoPanel;
        [SerializeField] Text infoPanelText;

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

        public void PickUpWeapon(GameObject weaponToPickUpObject, Vector3 originalLocation, int teamID, int weaponID, bool overrideLock = false)
        {
            //Move Weapon That's being picked up into place
            weaponToPickUpObject.transform.position = equippedWeaponPosition;
            weaponToPickUpObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            weaponToPickUpObject.transform.SetParent(Camera.main.transform);

            if(weapons[currentWeapon] != null) //If a weapon is equiped in current slot
            {  
                DropWeapon(currentWeapon);//drop current weapon
            }

            //Set current weapon to weapon that was just picked up
            weapons[currentWeapon] = weaponToPickUpObject.GetComponent<Weapon>();

            //Set up weapon
            weapons[weaponID].SetUp(teamID, weaponToPickUpObject, originalLocation);
        }
        public void DropWeapon(int weaponID)
        {
            if (weapons[weaponID].isWeaponDropable)
            {
                /* Vector3 forward = transform.forward;
                forward *= dropOffset.x;
                forward *= dropOffset.y;

                Vector3 dropLocation = transform.position + forward; */

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
            }
        }
        public void PickUpFlag()
        {
            foreach(var weapon in weapons)
            {
                weapon.gameObject.SetActive(false);
            }
            flagObject.SetActive(true);
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

        private void OnFirePerformed(InputAction.CallbackContext _context)
        {
            currentGun.Shoot();
        }
        private void OnFireCanceled(InputAction.CallbackContext _context)
        {

        }

        private void Start()
        {
            //Initial Weapon Set Up
            foreach (Weapon weapon in weapons)
            {
                weapon.SetUp(teamID, weapon.gameObject, Vector3.zero);
                weapon.gameObject.SetActive(false);
            }
            weapons[0].gameObject.SetActive(true);
            currentGun = weapons[0].GetComponent<Gun>();
            lastWeapon = 1;

            //Cursor Set Up
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Player Input Set Up
            charConrtol = gameObject.GetComponent<CharacterController>();

            playerInput = gameObject.GetComponent<PlayerInput>();

            reloadAction = playerInput.actions.FindAction("Reload");
            reloadAction.Enable();

            fireAction = playerInput.actions.FindAction("Fire");
            fireAction.Enable();
            fireAction.performed += OnFirePerformed;
            fireAction.performed += OnFireCanceled;



            //TESTING
            Debug.LogError("NOTE ERROR <DELETE AFTER READING>: added a weapon dropping system that works by dropping the actual object that the player has as a weapon" +
            ", however, the issue comes in when picking up the weapon and adding back to the player. Weapon needs a vector 3 that is it's position when attached/equiped"
            + " by the player. Or the vector 3 could be on the player, with all weapons going to the same location. Might not look great, but is definatley easier and more efficent");
        }
        private void Update()
        {
            //Testing
            if(!IsHoldingFlag)
            {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
                RaycastHit hit;
                Transform rayHit = null;
                if(Physics.Raycast(ray, out hit))
                {
                    rayHit = hit.transform;
                    if(rayHit.GetComponent<Weapon>())
                    {
                        infoPanelText.text = hit.transform.name;
                        infoPanel.SetActive(true);
                    }
                    else
                    {
                        infoPanel.SetActive(false);
                    }
                }
                if(reloadAction.ReadValue<float>() > 0)
                {
                    currentGun.StartReload();
                }
                if (fireAction.ReadValue<float>() > 0)
                {
                    currentGun.Shoot();
                }
                //if (Input.GetKeyDown(KeyCode.E))
                //{
                //    DropWeapon(currentWeapon);
                //    SwitchWeapon(lastWeapon, true);
                //}
                //if (Input.GetKeyDown(KeyCode.T))
                //{
                //    TakeDamage(1);
                //}
                //if(Input.GetKeyDown(KeyCode.C) && !currentGun.reloading)
                //{
                //    SwitchWeapon(lastWeapon);
                //}
                //if(Input.GetKeyDown(KeyCode.G) && rayHit != null)
                //{

                //}
            }
        }
    }
}