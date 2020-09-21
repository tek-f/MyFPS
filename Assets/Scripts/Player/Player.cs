using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
    public Vector3 dropOffset;

    #endregion
    private void SwitchWeapon(int weaponID, bool overrideLock = false)
    {
        if(!overrideLock && weapons[currentWeapon].isWeaponLocked == true)
        {
            return;
        }
        lastWeapon = currentWeapon;
        currentWeapon = weaponID;

        weapons[lastWeapon].gameObject.SetActive(false);
        weapons[currentWeapon].gameObject.SetActive(true);
    }

    public void PickUpWeapon(GameObject weaponObject, Vector3 originalLocation, int teamID, int weaponID, bool overrideLock = false)
    {
        SwitchWeapon(weaponID, overrideLock);

        weapons[weaponID].SetUp(teamID, weaponObject, originalLocation);
    }
    public void DropWeapon(int weaponID)
    {
        if(weapons[weaponID].isWeaponDropable)
        {
            Vector3 forward = transform.forward;
            forward *= dropOffset.x;
            forward *= dropOffset.y;

            Vector3 dropLocation = transform.position + forward;

            weapons[weaponID].DropWeapon(charConrtol, dropLocation);
            weapons[weaponID].worldWeaponGameObject.SetActive(true);

            SwitchWeapon(lastWeapon, true);
        }
    }
    public void ReturnWeapon(int weaponID)
    {
        if(weapons[weaponID].isWeaponDropable)
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
        if(currentWeapon == weaponID)
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
        if(health <= 0)
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
        /*foreach (Weapon weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }*/
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        charConrtol = gameObject.GetComponent<CharacterController>();
        SwitchWeapon(currentWeapon);
    }
    private void Update()
    {
        //Testing
        if(Input.GetKeyDown(KeyCode.E))
        {
            DropWeapon(currentWeapon);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(1);
        }
    }
}
