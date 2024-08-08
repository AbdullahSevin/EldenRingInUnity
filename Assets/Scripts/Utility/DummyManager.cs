using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AS
{
    public class DummyManager : MonoBehaviour
    {
        [SerializeField] bool networkSpawner;


        private void Update()
        {
            if (networkSpawner)
            {
                networkSpawner = false;
                // WE MUST FIRST SHUT DOWN THE NETWORK BECAUSE WE HAVE STARTED AS A HOST DURING THE TITLESCREEN
                NetworkManager.Singleton.Shutdown();
                // WE THEN RESTART AS A CLIENT
                NetworkManager.Singleton.StartClient();
            }
        }
   
    }
}

