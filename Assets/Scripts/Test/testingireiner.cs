using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFPS.Test
{
    public class testingireiner : MonoBehaviour
    {
        private void Start()
        {
            if(GetComponent<CharacterController>())
            {
                print("true");
            }
        }
    }
}