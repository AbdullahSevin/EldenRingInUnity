using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace AS
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        [Header("Damage Grunts")]
        [SerializeField] protected AudioClip[] damageGrunts;

        [Header("Attack     Grunts")]
        [SerializeField] protected AudioClip[] attackGrunts;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.PlayOneShot(soundFX, volume);
            //  RESETS AUDIO PITCH
            audioSource.pitch = 1;

            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }


        }
        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
        }

        public virtual void PlayDamageGrunt()
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
        }

        public virtual void PlayAttackGrunt()
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
        }




    }

}

