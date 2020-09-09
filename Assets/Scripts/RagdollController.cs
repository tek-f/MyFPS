using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    GameObject ragdoll;
    GameObject AI;

    List<Rigidbody> bodies;

    public bool test;

    private void Start()
    {
        SetKinematic(true);
    }

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

    private void Update()
    {
        if(test)
        {
            Death();
            test = false;
        }
    }
}
