using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace AS
{
    public class WorldGameSessionManager : MonoBehaviour
    {
        public static WorldGameSessionManager instance;

        [Header("Active Players In Session")]
        public List<PlayerManager> players = new List<PlayerManager>();


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

        public void AddPlayerToActivePlayersList(PlayerManager player)
        {

            //  CHECK LIST AND DO NOT ADD THE PLAYER IF ALREADY EXISTS
            if (!players.Contains(player))
            {
                players.Add(player);
            }


            //  CHECK THE LIST FOR NULL SLOTS AND REMVE NULL SLOTS

            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }


        }

        public void RemovePlayerFromActivePlayersList(PlayerManager player)
        {

            if (!players.Contains(player))
            {
                players.Remove(player);
            }

            for (int i = players.Count - 1; i > -1; i--)
            {
                if (players[i] == null)
                {
                    players.RemoveAt(i);
                }
            }


        }


    }
}

