using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AS
{
    public class WorldObjectManager : MonoBehaviour
    {
        public static WorldObjectManager instance;


        [Header("Network Objects")]
        [SerializeField] List<NetworkObjectSpawner> networkObjectSpawners;
        [SerializeField] List<GameObject> spawnedInObjects;


        [Header("Fog Walls")]
        public List<FogWallInteractable> fogWalls;
        //  Create an object script that will hold the logic for fog walls
        //  Spawn in those fogwalls as network objects during start of the game (must have a spammer object)
        //  Create general object spawner script and preafab
        //  When the fog walls are spawned add them to the fog wall list
        //  grab the correct fogwall from the list on the boss manager when the boss is being initialized.

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SpawnObject(NetworkObjectSpawner networkObjectSpawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                networkObjectSpawners.Add(networkObjectSpawner);
                networkObjectSpawner.AttemptToSpawnObject();
            }

        }

        public void AddFogWallToList(FogWallInteractable fogWall)
        {
            if (!fogWalls.Contains(fogWall))
            {
                fogWalls.Add(fogWall);
            }
        }

        public void RemoveFogWallFromList(FogWallInteractable fogWall)
        {
            if (fogWalls.Contains(fogWall))
            {
                fogWalls.Remove(fogWall);
            }
        }


    }
}

