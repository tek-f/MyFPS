using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFPS.Player
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] PlayerHandler player;
        [Header("Gun Metrics")]
        [SerializeField]
        int remainingAmmo, maxAmmo, totalAmmo;
        public float range = 100f, gunForce = 50f;
        public int damage = 2;
        public Camera fpsCamera;
        public ParticleSystem muzzleFlash;
        public GameObject impactEffect;
        public bool reloading = false, changing = false;
        [Header("HUD")]
        [SerializeField]
        Text ammoText, clipText;
        [Header("Animation")]
        public Animator anim;
        public void SetUp()
        {
            if (fpsCamera == null)
            {
                fpsCamera = Camera.main;
            }
            //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
            remainingAmmo = maxAmmo;
            totalAmmo = maxAmmo * 3;
            ammoText.text = remainingAmmo.ToString() + " / " + maxAmmo.ToString();
            clipText.text = totalAmmo.ToString();
        }
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
                    if (hit.transform.GetComponent<PlayerHandler>())
                    {
                        PlayerHandler hitPlayer = hit.transform.GetComponent<PlayerHandler>();
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

                    }
                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * gunForce, ForceMode.Impulse);
                    }
                }
            }
        }
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
        public void StartWeaponSwap()
        {
            if(!reloading && !changing)
            {
                anim.SetTrigger("swapWeapon");
                changing = true;
            }
        }
        public void TriggerWeaponSwap()
        {
            changing = false;
            player.SwitchWeapon(player.LastWeapon);
        }
        public void OnWeaponSwap()
        {
            ammoText.text = remainingAmmo.ToString() + " / " + maxAmmo.ToString();
            clipText.text = totalAmmo.ToString();
        }
        private void Start()
        {
            SetUp();
        }
        //private void Update()
        //{
        //    if (Input.GetButtonDown("Fire1") && !reloading)
        //    {
        //        Shoot();
        //    }
        //    if (Input.GetButtonDown("Reload"))
        //    {
        //        if (totalAmmo > 0 && remainingAmmo < maxAmmo)
        //        {
        //            StartReload();
        //        }
        //    }
        //}
    }
}