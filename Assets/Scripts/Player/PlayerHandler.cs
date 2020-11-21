using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using MyFPS.GameAdmin;

namespace MyFPS.Player
{
    public class PlayerHandler : MonoBehaviour
    {
        [SerializeField] GameObject pausePanel;
        [SerializeField] FirstPersonController fpsController;
        bool paused;
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
        InputAction swapWeaponAction;
        InputAction escapeAction;
        #endregion
        #region Weapons
        public List<Weapon> weapons;
        [SerializeField] GameObject flagObject;
        [SerializeField] int currentWeapon = 0, lastWeapon = 0;
        #region 
        public int LastWeapon
        {
            get
            {
                return lastWeapon;
            }
        }
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
        public void SwitchWeapon(int weaponID, bool overrideLock = false)
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
        public void ReturnFlag()
        {
            flagObject.SetActive(false);
            weapons[currentWeapon].gameObject.SetActive(true);
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
        void Teleport(Vector3 _position)
        {
            gameObject.transform.position = _position;
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
        public void Pause()
        {
            if(paused)
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pausePanel.SetActive(false);
                fpsController.enabled = true;
                paused = false;
            }
            else
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pausePanel.SetActive(true);
                fpsController.enabled = false;
                paused = true;
            }
        }
        private void OnFirePerformed(InputAction.CallbackContext _context)
        {
            currentGun.Shoot();
        }
        private void OnReloadPerformed(InputAction.CallbackContext _context)
        {
            currentGun.StartReload();
        }
        private void OnSwapWeaponPerformed(InputAction.CallbackContext _context)
        {
            currentGun.StartWeaponSwap();
        }
        private void OnEscapePerformed(InputAction.CallbackContext _context)
        {
            Pause();
        }

        private void Start()
        {
            //Variable/Refence SetUp
            fpsController = GetComponent<FirstPersonController>();

            //Initial Weapon Set Up
            foreach (Weapon weapon in weapons)
            {
                weapon.SetUp(teamID, weapon.gameObject, Vector3.zero);
                weapon.gameObject.SetActive(false);
            }
            weapons[0].gameObject.SetActive(true);
            currentGun = weapons[0].GetComponent<Gun>();
            lastWeapon = 1;

            //Temp Gun Setup

            //Cursor Set Up
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //Player Input Set Up
            charConrtol = gameObject.GetComponent<CharacterController>();

            playerInput = gameObject.GetComponent<PlayerInput>();

            reloadAction = playerInput.actions.FindAction("Reload");
            reloadAction.Enable();
            reloadAction.performed += OnReloadPerformed;

            fireAction = playerInput.actions.FindAction("Fire");
            fireAction.Enable();
            fireAction.performed += OnFirePerformed;

            swapWeaponAction = playerInput.actions.FindAction("SwapWeapon");
            swapWeaponAction.Enable();
            swapWeaponAction.performed += OnSwapWeaponPerformed;

            escapeAction = playerInput.actions.FindAction("Escape");
            escapeAction.Enable();
            escapeAction.performed += OnEscapePerformed;
        }
        private void Update()
        {
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
                //if(reloadAction.ReadValue<float>() > 0)
                //{
                //    currentGun.StartReload();
                //}
                //if (fireAction.ReadValue<float>() > 0)
                //{
                //    currentGun.Shoot();
                //}
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
        #endregion
    }
}