using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    /// <summary>
    /// Targets health.
    /// </summary>
    public float health = 100f;
    /// <summary>
    /// Reduces health by _damage. Also destroys game object if health is then lower then 1.
    /// </summary>
    /// <param name="_damage">The value health is reduced by.</param>
    public void TakeDamage(float _damage)
    {
        health -= _damage;
        if(health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
