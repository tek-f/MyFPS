using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    /// <summary>
    /// Reference to the ragdoll that is set active when the parent object "dies".
    /// </summary>
    GameObject ragdoll;
    /// <summary>
    /// List of rigidbodies that make up the ragdoll.
    /// </summary>
    List<Rigidbody> bodies;
    /// <summary>
    /// Ragdolls health.
    /// </summary>
    [SerializeField] int health = 10;
    /// <summary>
    /// Sets all rigidbodies in bodies[] to _valueToSet.
    /// </summary>
    /// <param name="_valueToSet">Value to set rigidbodies isKinematic too.</param>
    void SetKinematic(bool _valueToSet)
    {
        bodies = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());

        foreach (Rigidbody body in bodies)
        {
            body.isKinematic = _valueToSet;
        }
    }
    /// <summary>
    /// On the death of the parent of the ragdoll.
    /// </summary>
    public void Death()
    {
        SetKinematic(false);
        Animator anim;
        if(TryGetComponent<Animator>(out anim))
        {
            anim.enabled = false;
        }
    }
    /// <summary>
    /// Reduces health by _damage.
    /// </summary>
    /// <param name="_damage">Value to reduce health by.</param>
    public void TakeDamage(int _damage)
    {
        health -= _damage;
        Debug.Log("damage taken = " + _damage);
        if(health <= 0)
        {
            Death();
        }
    }
    private void Start()
    {
        SetKinematic(true);
    }
}
