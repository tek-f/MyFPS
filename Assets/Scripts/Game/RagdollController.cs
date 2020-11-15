using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    GameObject ragdoll;
    List<Rigidbody> bodies;
    [SerializeField] int health = 10;

    void SetKinematic(bool valueToSet)
    {
        bodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());

        foreach (Rigidbody body in bodies)
        {
            body.isKinematic = valueToSet;
        }
    }
    public void Death()
    {
        SetKinematic(false);
        Animator anim;
        if(TryGetComponent<Animator>(out anim))
        {
            anim.enabled = false;
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("damage taken = " + damage);
        if(health <= 0)
        {
            Death();
            print("ded");
        }
    }
    private void Start()
    {
        SetKinematic(true);
    }
}
