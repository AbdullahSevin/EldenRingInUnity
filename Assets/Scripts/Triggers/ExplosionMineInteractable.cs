using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class ExplosionMineInteractable : Interactable
    {

        public GameObject explosionVFX;
        public float explosionSize = 10;



        public override void OnTriggerEnter(Collider other)
        {
            PlayerManager player;
            player = other.GetComponent<PlayerManager>();

            if (player == null)
            {
                return;
            }
            
            GameObject explosionInstance = Instantiate(explosionVFX, player.transform.position, player.transform.rotation);

            // Access the ParticleSystem component
            ParticleSystem particleSystem = explosionInstance.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                var main = particleSystem.main;
                main.startSize = explosionSize;
            }
            base.Interact(player);
            

            Destroy(gameObject, 1);
        }



    }
}

