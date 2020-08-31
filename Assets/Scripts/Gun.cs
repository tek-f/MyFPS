using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{
    public float damage = 10f, range = 100f;
    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public 
    void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log("Fired");

            Target target = hit.transform.GetComponent<Target>();

            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if(impactEffect != null)
            {
                GameObject Impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal), hit.transform);
                Destroy(Impact, 10f);
            }
        }
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
