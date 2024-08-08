using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;

        [Header("Action Sounds")]
        public AudioClip rollSFX;

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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);
            return array[index];
        }
    }


}

