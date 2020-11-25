using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyFPS.Test;

namespace MyFPS.Player
{
    public class Gun : MonoBehaviour
    {
        /// <summary>
        /// Reference to the player that has this Gun in their loadout, if any.
        /// </summary>
        [SerializeField] PlayerHandler player;
        [Header("Gun Metrics")]
        /// <summary>
        /// The amount of ammo this gun has remaining in this magazine.
        /// </summary>
        [SerializeField] int remainingAmmo;
        /// <summary>
        /// The maximum amount of ammo this gun can hold in a magazine.
        /// </summary>
        [SerializeField] int maxAmmo;
        /// <summary>
        /// The total pool of ammo this gun has to reload from.
        /// </summary>
        [SerializeField] int totalAmmo;
        /// <summary>
        /// The range of this gun.
        /// </summary>
        public float range = 100f;
        /// <summary>
        /// The force that is applied when this gun shoots an object with a rigidbody.
        /// </summary>
        public float gunForce = 50f;
        /// <summary>
        /// The damage this gun does to targets and players.
        /// </summary>
        public int damage = 2;
        /// <summary>
        /// Reference to the camera for the player that this gun is equiped by.
        /// </summary>
        public Camera fpsCamera;
        /// <summary>
        /// The particle system for this gun that is the muzzle flash.
        /// </summary>
        public ParticleSystem muzzleFlash;
        /// <summary>
        /// Prefab used to make the impact effect for this gun, aka bullet holes.
        /// </summary>
        public GameObject impactEffect;
        /// <summary>
        /// Tracks if this gun is being reloaded.
        /// </summary>
        public bool reloading = false;
        /// <summary>
        /// Tracks if this gun is being swapped out.
        /// </summary>
        public bool changing = false;
        [Header("HUD")]
        /// <summary>
        /// Reference to the UI Text that displays totalAmmo.
        /// </summary>
        [SerializeField] Text ammoText;
        /// <summary>
        /// Reference to the UI Text that displays remainingAmmo.
        /// </summary>
        [SerializeField] Text clipText;
        [Header("Animation")]
        /// <summary>
        /// Reference to the Animator component on this game object.
        /// </summary>
        public Animator anim;
        /// <summary>
        /// Set Up function for the Gun.
        /// </summary>
        public void SetUp()
        {
            if (fpsCamera == null)
            {
                fpsCamera = Camera.main;
            }
            //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
            remainingAmmo = maxAmmo;
            totalAmmo = maxAmmo * 3;
        }

        public void UpdateAmmoUI()
        {
            ammoText.text = remainingAmmo.ToString() + " / " + maxAmmo.ToString();
            clipText.text = totalAmmo.ToString();
        }
        /// <summary>
        /// Firing function for the gun.
        /// </summary>
        public void Shoot()
        {
            if (remainingAmmo > 0 && !reloading && !changing)
            {
                remainingAmmo--;
                ammoText.text = remainingAmmo.ToString() + " / " + maxAmmo.ToString();
                if (muzzleFlash != null)
                {
                    muzzleFlash.Play();
                }
                RaycastHit hit;
                if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        print("player object hit");
                        PlayerHandler hitPlayer = hit.transform.GetComponent<PlayerHandler>();
                        print(hitPlayer);
                        hitPlayer.TakeDamage(damage);

                        /*Has been replaced by Takedamage() and Death() functions in Player class
                         * 
                        RagdollController ragdoll;
                        if (hit.transform.gameObject.TryGetComponent<RagdollController>(out ragdoll))
                        {
                            Debug.Log("Ragdoll fouond");
                            ragdoll.Death();
                        }*/
                    }
                    else
                    {
                        if(hit.transform.GetComponent<RagdollController>())
                        {
                            hit.transform.GetComponent<RagdollController>().TakeDamage(damage);
                        }
                        if (hit.transform.GetComponent<Target>())
                        {
                            Target target = hit.transform.GetComponent<Target>();
                            target.TakeDamage(Random.Range(damage / 2, damage));
                        }

                        if (impactEffect != null)
                        {
                            GameObject Impact = Instantiate(impactEffect, hit.point + new Vector3(0, 0.01f, 0), Quaternion.LookRotation(hit.normal), hit.transform);
                            Destroy(Impact, 10f);
                        }
                        //Testing
                        if(hit.transform.GetComponent<DummyPlayer>())
                        {
                            hit.transform.GetComponent<DummyPlayer>().health -= damage;
                        }

                    }
                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * gunForce, ForceMode.Impulse);
                    }
                }
            }
        }
        /// <summary>
        /// Reload function for the gun.
        /// </summary>
        public void Reload()
        {
            if(totalAmmo < (maxAmmo - remainingAmmo))            
            {
                remainingAmmo += totalAmmo;
                totalAmmo = 0;
            }
            else if(totalAmmo - (maxAmmo - remainingAmmo) < 0)
            {
                remainingAmmo = totalAmmo;
                totalAmmo = 0;
            }
            else
            {
                totalAmmo -= (maxAmmo - remainingAmmo);
                remainingAmmo = maxAmmo;
            }
            
            ammoText.text = remainingAmmo.ToString() + " / " + maxAmmo.ToString();
            clipText.text = totalAmmo.ToString();
            reloading = false;
        }
        /// <summary>
        /// Triggers reload annimation and begins reloading.
        /// </summary>
        public void StartReload()
        {
            if(!reloading && !changing)
            {
                if (totalAmmo > 0 && remainingAmmo == 0)
                {
                    anim.SetTrigger("reloadEmpty");
                    reloading = true;
                }
                else if (totalAmmo > 0 && remainingAmmo < maxAmmo)
                {
                    anim.SetTrigger("reload");
                    reloading = true;
                }
            }
        }
        /// <summary>
        /// Triggers weapon swap annimation and begins swapping.
        /// </summary>
        public void StartWeaponSwap()
        {
            if(!reloading && !changing)
            {
                anim.SetTrigger("swapWeapon");
                changing = true;
            }
        }
        /// <summary>
        /// Weapon swap funciton for the gun.
        /// </summary>
        public void TriggerWeaponSwap()
        {
            changing = false;
            player.SwitchWeapon(player.LastWeapon);
        }
        /// <summary>
        /// Updates UI to current guns ammo details.
        /// </summary>
        public void OnWeaponSwap()
        {
            ammoText.text = remainingAmmo.ToString() + " / " + maxAmmo.ToString();
            clipText.text = totalAmmo.ToString();
        }
        private void Start()
        {
            SetUp();
            UpdateAmmoUI();
        }
    }
}