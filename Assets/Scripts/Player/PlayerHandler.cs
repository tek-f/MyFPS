using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MyFPS.GameAdmin;
using MyFPS.Mirror;
using Mirror;

namespace MyFPS.Player
{
    public class PlayerHandler : NetworkBehaviour
    {
        #region Properties
        [SyncVar] public int teamID;
        public int LastWeapon
        {
            get
            {
                return lastWeapon;
            }
        }
        public bool IsHoldingFlag
        {
            get
            {
                return flagObject.activeSelf;
            }
        }
        #endregion
        #region Player Metrics
        /// <summary>
        /// Players health.
        /// </summary>
        public int health = 15;
        /// <summary>
        /// The players max health.
        /// </summary>
        public int maxHealth = 15;
        /// <summary>
        /// Reference to the FirstPersonController class on this game object.
        /// </summary>
        [SerializeField] FirstPersonController fpsController;
        /// <summary>
        /// Reference to the CharacterController component on this game object.
        /// </summary>
        CharacterController charConrtol;
        #endregion
        #region Game Mode
        /// <summary>
        /// Reference to this games GameMode.
        /// </summary>
        GameMode gameModeManager;
        /// <summary>
        /// This players team id.
        /// </summary>
        [SerializeField] int playersTeamID;
        #endregion
        #region Input
        /// <summary>
        /// Reference to the PlayerInput component on this game object.
        /// </summary>
        [SerializeField] PlayerInput playerInput;
        /// <summary>
        /// Reload action on the player input map.
        /// </summary>
        InputAction reloadAction;
        /// <summary>
        /// Fire action on the player input map.
        /// </summary>
        InputAction fireAction;
        /// <summary>
        /// Swap action on the player input map.
        /// </summary>
        InputAction swapWeaponAction;
        /// <summary>
        /// Escape action on the player input map.
        /// </summary>
        InputAction escapeAction;
        #endregion
        #region Weapons
        /// <summary>
        /// List<> of Weapon class that is the players loadout.
        /// </summary>
        public List<Weapon> weapons;
        /// <summary>
        /// This players local flag object.
        /// </summary>
        [SerializeField] GameObject flagObject;
        /// <summary>
        /// Index to weapons<> of the current weapon the player has equiped.
        /// </summary>
        [SerializeField] int currentWeapon = 0;
        /// <summary>
        /// Index to weapons<> of the previous weapon the player had equiped.
        /// </summary>
        [SerializeField] int lastWeapon = 0;
        /// <summary>
        /// Reference to the Gun class on the equiped weapon.
        /// </summary>
        Gun currentGun;
        /// <summary>
        /// Offset used when a weapon is dropped.
        /// </summary>
        public Vector3 dropOffset;
        /// <summary>
        /// Position the weapon is set to when it is equiped.
        /// </summary>
        public Vector3 equippedWeaponPosition;
        #region GUI
        /// <summary>
        /// Reference to the Text that displays team 1's score.
        /// </summary>
        [SerializeField] Text team1ScoreText;
        /// <summary>
        /// Reference to the Text that displays team 1's score.
        /// </summary>
        [SerializeField] Text team2ScoreText;
        /// <summary>
        /// Reference to the players Pause Menu panel.
        /// </summary>
        [SerializeField] GameObject pausePanel;
        /// <summary>
        /// Reference to the players Loadout panel.
        /// </summary>
        [SerializeField] GameObject loadoutPanel;
        /// <summary>
        /// Tracks if player has paused the game.
        /// </summary>
        bool paused;
        /// <summary>
        /// Reference to the end game display panel.
        /// </summary>
        public GameObject endGamePanel;
        /// <summary>
        /// Reference to the image that is the players crosshair.
        /// </summary>
        public Image crosshair;
        #endregion
        #region Respawn
        /// <summary>
        /// Position for the player to respaw to.
        /// </summary>
        [SyncVar] public Vector3 respawnPosition;
        /// <summary>
        /// Reference to the PlayerDeath class on this game object.
        /// </summary>
        [SerializeField] PlayerDeath playerDeath;
        #endregion
        #endregion

        [ClientRpc]
        public void RpcSetRespawnPos(Vector3 _respawnTransform)
        {
            respawnPosition = _respawnTransform;
        }
        /// <summary>
        /// Swaps the players equpied weapon.
        /// </summary>
        /// <param name="_weaponID">Weapon ID of the weapon to be equpied.</param>
        /// <param name="_overrideLock">Determines if the weapon is able to be swapped. Default = false.</param>
        public void SwitchWeapon(int _weaponID, bool _overrideLock = false)
        {
            if (weapons[_weaponID] != null && !_overrideLock && weapons[currentWeapon].isWeaponLocked == true)
            {
                return;
            }
            lastWeapon = currentWeapon;
            currentWeapon = _weaponID;

            if(weapons[lastWeapon] != null)
            {
                weapons[lastWeapon].gameObject.SetActive(false);
            }
            weapons[currentWeapon].gameObject.SetActive(true);
            currentGun = weapons[currentWeapon].GetComponent<Gun>();
            currentGun.OnWeaponSwap();
        }
        /// <summary>
        /// Picks up a weapon from a world space weapon object and adds it to this players weapons<>.
        /// </summary>
        /// <param name="_weaponToPickUpObject">Object of the weapon to be picked up.</param>
        /// <param name="_originalLocation">Original locaiton of the weapon to be picked up.</param>
        /// <param name="_teamID">Team ID of the weapon to be picked up.</param>
        /// <param name="_weaponID">Weapon ID of the weapon to be picked up.</param>
        /// <param name="_overrideLock">Determines if the weapon is able to be swapped. Default = false.</param>
        public void PickUpWeapon(GameObject _weaponToPickUpObject, Vector3 _originalLocation, int _teamID, int _weaponID, bool _overrideLock = false)
        {
            //Move Weapon That's being picked up into place
            _weaponToPickUpObject.transform.position = equippedWeaponPosition;
            _weaponToPickUpObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            _weaponToPickUpObject.transform.SetParent(Camera.main.transform);

            if(weapons[currentWeapon] != null) //If a weapon is equiped in current slot
            {  
                DropWeapon(currentWeapon);//drop current weapon
            }

            //Set current weapon to weapon that was just picked up
            weapons[currentWeapon] = _weaponToPickUpObject.GetComponent<Weapon>();

            //Set up weapon
            weapons[_weaponID].SetUp(_teamID, _weaponToPickUpObject, _originalLocation);
        }
        /// <summary>
        /// Drops weapon and removes it from this players weapons<>.
        /// </summary>
        /// <param name="_weaponID">Weapon ID of the weapon to be dropped.</param>
        public void DropWeapon(int _weaponID)
        {
            if (weapons[_weaponID].isWeaponDropable)
            {
                /* Vector3 forward = transform.forward;
                forward *= dropOffset.x;
                forward *= dropOffset.y;

                Vector3 dropLocation = transform.position + forward; */

                weapons[_weaponID].DropWeapon(charConrtol);
                
                weapons[currentWeapon] = null;
                SwitchWeapon(lastWeapon, true);
            }
        }
        /// <summary>
        /// Returns weapon to it's original locaiton.
        /// </summary>
        /// <param name="_weaponID">Weapon ID of the weapon to be returned.</param>
        public void ReturnWeapon(int _weaponID)
        {
            if (weapons[_weaponID].isWeaponDropable)
            {
                Vector3 returnLocation = weapons[_weaponID].originalLocation;

                weapons[_weaponID].worldWeaponGameObject.transform.position = returnLocation;
                weapons[_weaponID].worldWeaponGameObject.SetActive(true);
            }
        }
        /// <summary>
        /// Returns flag to ir's original location.
        /// </summary>
        public void ReturnFlag()
        {
            flagObject.SetActive(false);
            fireAction.Enable();
            weapons[currentWeapon].enabled = true;
            weapons[currentWeapon].gameObject.SetActive(true);
        }
        /// <summary>
        /// Picks up flag.
        /// </summary>
        public void PickUpFlag()
        {
            fireAction.Disable();
            foreach (var weapon in weapons)
            {
                weapon.gameObject.SetActive(false);
            }
            weapons[currentWeapon].enabled = false;
            flagObject.SetActive(true);
        }
        /// <summary>
        /// Returns the team ID of the current weapon equiped.
        /// </summary>
        /// <returns>Integer.</returns>
        public int GetWeaponTeamID()
        {
            return weapons[currentWeapon].teamID;
        }
        /// <summary>
        /// Reterns true if the player is holding weapon that has specific weapon ID.
        /// </summary>
        /// <param name="_weaponID">Weapon ID to check against.</param>
        /// <returns>Bool.</returns>
        public bool IsHolding(int _weaponID)
        {
            if (currentWeapon == _weaponID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Teleports the player to position.
        /// </summary>
        /// <param name="_position">Position player is teleported to.</param>
        void Teleport(Vector3 _position)
        {
            gameObject.transform.position = _position;
        }
        /// <summary>
        /// Reduces players health by value.
        /// </summary>
        /// <param name="_damage">Value.</param>
        public void TakeDamage(int _damage)
        {
            print("Damage taken, " + _damage);
            health -= _damage;
            print("health remaining: " + health);
            if (health <= 0)
            {
                Death();
            }
        }

        [ClientRpc]
        public void RpcSetTeamID(int _teamID)
        {
            teamID = _teamID;
        }
        /// <summary>
        /// Called when players health reaches 0. Enables playerDeath.
        /// </summary>
        public void Death()
        {
            if (hasAuthority)
            {
                //set death script active
                playerDeath.enabled = true;
                foreach (Weapon weapon in weapons)
                {
                    weapon.GetComponent<Gun>().SetUp();
                }
                currentGun.UpdateAmmoUI();
                print("Player death method");
                if (gameModeManager.gameType == "DM")
                {
                    CmdUpdateTeamScores(teamID);
                }
            }
        }
        //[ClientRpc]
        //public void RpcDeath()
        //{
        //    Debug.Log("Rpc Player Death");
        //    Death();
        //}
        //[Command]
        //public void CmdDeath()
        //{
        //    Debug.Log("Player death command");
        //    RpcDeath();
        //}
        [Command]
        public void CmdUpdateTeamScores(int _teamID)
        {
            GameMode.instance.RpcUnpdateScoreNetwork(_teamID);
        }
        /// <summary>
        /// Updates UI display of the teams scores.
        /// </summary>
        /// <param name="_team1Score">Score of team 1.</param>
        /// <param name="_team2Score">Score of team 2.</param>
        public void UpdateTeamScores(int _team1Score, int _team2Score)
        {
            team1ScoreText.text = _team1Score.ToString();
            team2ScoreText.text = _team2Score.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        public void AddScore()
        {
            gameModeManager.UpdateScores(teamID);
        }
        /// <summary>
        /// Toggles game pause.
        /// </summary>
        public void Pause()
        {
            if(paused)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pausePanel.SetActive(false);
                fpsController.enabled = true;
                fireAction.Enable();
                paused = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pausePanel.SetActive(true);
                fpsController.enabled = false;
                fireAction.Disable();
                paused = true;
            }
        }
        /// <summary>
        /// Action to be performed when fireAction is performed.
        /// </summary>
        /// <param name="_context">Context of Input Action.</param>
        public void Exit()
        {
            if(NetworkManagerLobby.instance.isHost)
            {
                NetworkManagerLobby.instance.StopHost();
            }
            if (!NetworkManagerLobby.instance.isHost)
            {
                NetworkManagerLobby.instance.StopClient();
            }
            //SceneManager.LoadScene(0);
        }
        private void OnFirePerformed(InputAction.CallbackContext _context)
        {
            CmdShoot();
        }
        [Command]
        void CmdShoot()
        {
            RpcShoot();
        }
        [ClientRpc]
        void RpcShoot()
        {
            currentGun.Shoot();
        }
        /// <summary>
        /// Action to be performed when reloadAction is performed.
        /// </summary>
        /// <param name="_context">Context of Input Action.</param>
        private void OnReloadPerformed(InputAction.CallbackContext _context)
        {
            currentGun.StartReload();
        }
        /// <summary>
        /// Action to be performed when swapWeaponAction is performed.
        /// </summary>
        /// <param name="_context">Context of Input Action.</param>
        private void OnSwapWeaponPerformed(InputAction.CallbackContext _context)
        {
            //currentGun.StartWeaponSwap();
            fpsController.enabled = false;
            fireAction.Disable();
            loadoutPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        /// <summary>
        /// Action to be performed when swapWeaponAction is canceled/released.
        /// </summary>
        /// <param name="_context">Context of the Input Action.</param>
        private void OnSwapWeaponCanceled(InputAction.CallbackContext _context)
        {
            //currentGun.StartWeaponSwap();
            fpsController.enabled = true;
            loadoutPanel.SetActive(false);
            fireAction.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        /// <summary>
        /// Action to be performed when escapeAction is performed.
        /// </summary>
        /// <param name="_context">Context of Input Action.</param>
        private void OnEscapePerformed(InputAction.CallbackContext _context)
        {
            Pause();
        }
        private void Awake()
        {
            //GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            //foreach (GameObject _spawnPoint in spawnPoints)
            //{
            //    PlayerSpawnPoint possibleSpawnPoint = _spawnPoint.GetComponent<PlayerSpawnPoint>();
            //    if(possibleSpawnPoint != null && possibleSpawnPoint.teamID == teamID)
            //    {
            //        transform.position = _spawnPoint.transform.position;
            //    }
            //}

            gameModeManager = GameMode.instance;

            //Variable/Refence SetUp
            //fpsController = GetComponent<FirstPersonController>();
            //playerDeath = GetComponent<PlayerDeath>();


            //Add player to gameModeManager list of players
            gameModeManager.playersList.Add(this);

            //Initial Weapon Set Up
            foreach (Weapon weapon in weapons)
            {
                weapon.SetUp(teamID, weapon.gameObject, Vector3.zero);
                weapon.gameObject.SetActive(false);
            }
            weapons[0].gameObject.SetActive(true);
            currentGun = weapons[0].GetComponent<Gun>();
            lastWeapon = 1;

        }
        private void Start()
        {
            respawnPosition = transform.position;

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
            swapWeaponAction.canceled += OnSwapWeaponCanceled;

            escapeAction = playerInput.actions.FindAction("Escape");
            escapeAction.Enable();
            escapeAction.performed += OnEscapePerformed;
        }
        private void Update()
        { 
        
            if(!IsHoldingFlag)
            {
                //TESTING ONLY
                if(Input.GetKeyDown(KeyCode.T))//if T is pressed
                {
                    TakeDamage(5);//player is killed
                }

                #region Old Code
                //Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
                //RaycastHit hit;
                //Transform rayHit = null;
                //if(Physics.Raycast(ray, out hit))
                //{
                //    rayHit = hit.transform;
                //    if(rayHit.GetComponent<Weapon>())
                //    {
                //        infoPanelText.text = hit.transform.name;
                //        infoPanel.SetActive(true);
                //    }
                //    else
                //    {
                //        infoPanel.SetActive(false);
                //    }
                //}
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
                #endregion
            }
        }
    }
}