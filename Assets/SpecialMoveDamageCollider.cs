using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class SpecialMoveDamageCollider : DamageCollider
    {

        void OnParticleCollision(GameObject other)
        {
            if (other.tag == "Player")
            {
                // ignore colission
            }
            Debug.Log(other.name);
        }

        //public float velocityAmount = 10f;  // The amount of velocity to add

        //void OnParticleCollision(GameObject other)
        //{
        //    Rigidbody rb = other.GetComponent<Rigidbody>();
        //    if (rb != null)
        //    {
        //        // Calculate the direction to set the velocity
        //        Vector3 velocityDirection = other.transform.position - transform.position;
        //        velocityDirection.Normalize();

        //        // Directly set the Rigidbody's velocity
        //        rb.velocity = velocityDirection * velocityAmount;
        //    }
        //}


    }
}

