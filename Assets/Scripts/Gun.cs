using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Gun : MonoBehaviour
{
    [SerializeField]
    int remainingAmmo, maxAmmo, clipCount;
    public float damage = 10f, range = 100f, gunForce = 50f;
    public Camera fpsCamera;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    [Header("HUD")]
    [SerializeField]
    Text ammoText, clipText;
    [Header("Animation")]
    public Animator anim;
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
    }
    void Reload()
    {
        clipCount--;
        remainingAmmo = maxAmmo;
        ammoText.text = remainingAmmo.ToString() + " / " + maxAmmo.ToString();
        clipText.text = "Clips: " + clipCount.ToString();
        anim.SetTrigger("reload");
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
        remainingAmmo = maxAmmo;
        clipCount = 3;
        ammoText.text = remainingAmmo.ToString() + " / " + maxAmmo.ToString();
        clipText.text = "Clips: " + clipCount.ToString();
    }
    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(clipCount > 0)
            {
                Reload();
            }
        }
    }
}
