using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gun : MonoBehaviour
{
    int remainingAmmo, maxAmmo;

    public float damage = 10f, range = 100f, gunForce = 50f;
    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    [Header("HUD")]
    [SerializeField]
    Text ammoText;
    void Shoot()
    {
        if (remainingAmmo > 0)
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
                Debug.Log(hit.transform.name);

                Target target = hit.transform.GetComponent<Target>();

                if (target != null)
                {
                    target.TakeDamage(Random.Range(damage / 2, damage));
                }

                if (impactEffect != null)
                {
                    GameObject Impact = Instantiate(impactEffect, hit.point + new Vector3(0, 0.01f, 0), Quaternion.LookRotation(hit.normal), hit.transform);
                    Destroy(Impact, 10f);
                }
                RagdollController ragdoll;
                if (hit.transform.gameObject.TryGetComponent<RagdollController>(out ragdoll))
                {
                    Debug.Log("Ragdoll fouond");
                    ragdoll.Death();
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * gunForce, ForceMode.Impulse);
                }
            }
        }
        else
        {
            Reload();
        }
    }
    void Reload()
    {
        remainingAmmo = maxAmmo;
        ammoText.text = remainingAmmo.ToString() + " / " + maxAmmo.ToString();
    }
    private void Start()
    {
        if(fpsCamera == null)
        {
            fpsCamera = GetComponent<Camera>();
        }
        if (fpsCamera == null)
        {
            fpsCamera = Camera.main;
        }
    }
    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
}
