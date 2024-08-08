using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {

        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        private void OnAnimatorMove()
        {
            if (player.applyRootMotion)
            {
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation;
            }
        }

        //  ANIMATION EVENT CALLS
        public override void EnableCanDoCombo()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = true;
            }
            else
            {
                //  ENABLE OFF HAND COMBO
            }
        }


        public override void DisableCanDoCombo()
        {
            player.playerCombatManager.canComboWithMainHandWeapon = false;
            // player.playerCombatManager.canComboWithOffHandWeapon = false;

        }




    }

}
