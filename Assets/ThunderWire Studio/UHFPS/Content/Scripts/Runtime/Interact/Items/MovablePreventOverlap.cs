using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UHFPS.Runtime
{
    public class MovablePreventOverlap : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
        }

        private void OnCollisionExit(Collision collision)
        {
            
        }
    }
}