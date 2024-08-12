using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    [System.Serializable]
    // SINCE WE WANT TO REFERENCE THIS DATA FOR EVERY SAVE FILE, THIS SCRIPT IS NOT A MONOBEHAVIOUR AND IS INSTEAD SERIALIZABLE
    public class CharacterSaveData
    {
        [Header("SCENE INDEX")]
        public int sceneIndex = 1;


        [Header("Character Name")]
        public string characterName = "Character";

        [Header("Time Played")]
        public float secondsPlayed;


        // QUESTION: WHY NOT SAVE VECTOR3?
        // ANSWER: WE CAN ONLY SAVE DATA FROM "BASIC" VARIABLE TYPES (float, int, string, bool, ect)
        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;


        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;

        [Header("Stats")]
        public int vitality;
        public int endurance;

        [Header("Bosses")]
        public SerializableDictionary<int, bool> bossesAwakened;  //  The int is boss id, bool is the awakened status
        public SerializableDictionary<int, bool> bossesDefeated;  //  The int is boss id, bool is the defeated status


        public CharacterSaveData()
        {
            bossesAwakened = new SerializableDictionary<int, bool>();
            bossesDefeated = new SerializableDictionary<int, bool>();
        }

    }
}

