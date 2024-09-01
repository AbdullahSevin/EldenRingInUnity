using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class PlayerSpecialMovesManager : CharacterSpecialMovesManager
    {

        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public override void AttemptToPerformSpecialMove(SpecialMove specialMove)
        {

            if (specialMove == SpecialMove.Kamehameha)
            {
                if (player.isPerformingAction == true)
                {
                    return;
                }

                if (player.playerNetworkManager.isPerformingSpecialMove.Value)
                {
                    return;
                }



                character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Kamehameha_01", true);

                if (player.IsOwner)
                {
                    Debug.Log("isowner");
                    player.isPerformingAction = true;
                    player.playerNetworkManager.isInvulnerable.Value = true;
                    player.playerNetworkManager.isAttacking.Value = true;
                    player.playerNetworkManager.isPerformingSpecialMove.Value = true;
                    player.playerNetworkManager.isPerformingKamehameha.Value = true;
                }

            }
            if (specialMove == SpecialMove.Kienzan)
            {
                Debug.Log("Kienzan Released");
            }

            if (specialMove == SpecialMove.FinalFlash)
            {
                Debug.Log("Final Flash Released");
            }


        }


    }
}

