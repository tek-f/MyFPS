using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MirrorPlayerHandler : NetworkBehaviour
{
    [SerializeField] private Vector3 movement = new Vector3();



    private void Update()
    {
        if(Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(movement * Input.GetAxis("Horizontal"));
        }
    }
}
