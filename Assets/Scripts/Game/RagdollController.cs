using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    GameObject ragdoll;
    List<Rigidbody> bodies;

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
}
