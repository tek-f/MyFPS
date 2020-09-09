using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Game Mode
    [SerializeField] int playersTeamID;
    public int teamID { get { return playersTeamID; } }

    Rigidbody playerRB; //players rigid body component
    CharacterController playerChar;
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

            weapons[weaponID].DropWeapon(playerChar, forward);
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

    private void Start()
    {
        /*foreach (Weapon weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }*/

        SwitchWeapon(currentWeapon);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            DropWeapon(1);
            Debug.Log("weapon dropped");
        }
    }
}
