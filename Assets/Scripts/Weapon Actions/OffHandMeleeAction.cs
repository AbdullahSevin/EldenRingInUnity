using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Off Hand Melee Action")]
    public class OffHandMeleeAction : WeaponItemAction
    {
        // Q: WHY CALL IT OFF HAND MELEE ACTION AND NOT BLOCK ACTION
        // A: IN THE FUTURE WE WILL HAVE DUAL ATTACKS WITH USING 2 WEAPONS ON BOTH MAIN AND OFF HANDS

        public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);

            //  CHECK FOR POWER STANCE ACTION (DUAL ATTACK)

            // CHECK FOR CAN BLOCK
            if (!playerPerformingAction.playerCombatManager.canBlock)
            {
                return;
            }

            // CHECK FOR ATTACK STATUS
            if (playerPerformingAction.playerNetworkManager.isAttacking.Value)
            {
                // DISABLE BLOCKING (When using a short/medium spear block attacking is allowed wih light attacks. Handled on another action class)
                if (playerPerformingAction.IsOwner)
                {
                    playerPerformingAction.playerNetworkManager.isBlocking.Value = false;
                }

                return;
            }

            if (playerPerformingAction.playerNetworkManager.isBlocking.Value)
            {
                return;
            }

            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction.playerNetworkManager.isBlocking.Value = true;
            }


        }

    }
}

