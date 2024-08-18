using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class PlayerSoundFXManager : CharacterSoundFXManager
    {
        PlayerManager player;

        [Header("FootSteps")]
        [SerializeField] public AudioClip[] pc_footStepR;
        [SerializeField] public AudioClip[] pc_footStepL;

        [Header("JumpSFX")]
        [SerializeField] public AudioClip[] pc_jumpSoundFX;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }
        public override void PlayBlockSoundFX()
        {
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(player.playerCombatManager.currentWeaponBeingUsed.blocking));
        }


        private void FootR()
        {
            //PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(pc_footStepR));
        }

        private void FootL()
        {
            //PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(pc_footStepL));
        }

        private void PlayJumpSoundFX()
        {
            if (pc_jumpSoundFX.Length > 0)
            {
                PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(pc_jumpSoundFX));
            }
           
        }
    }

}

