using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    [CreateAssetMenu(menuName = "Items/Ash Of War/Parry")]
    public class ParryAshOfWar : AshOfWar
    {

        public override void AttempToPerformAction(PlayerManager playerPerformingAction)
        {
            base.AttempToPerformAction(playerPerformingAction);

            if (!CanIUseThisAbility(playerPerformingAction))
            {
                
                return;
            }
            DeductStaminaCost(playerPerformingAction);
            DeductFocusPointCost(playerPerformingAction);
            PerformParryTypeBasedOnWeapon(playerPerformingAction);

        }

        public override bool CanIUseThisAbility(PlayerManager playerPerformingAction)
        {
            if (playerPerformingAction.isPerformingAction)
            {
                Debug.Log("CAN NOT PERFORM ASH OF WAR -- -  --  U R ALREADY PERFORMING AN ACTION");
                return false;
            }

            if (playerPerformingAction.playerNetworkManager.isJumping.Value)
            {
                Debug.Log("CAN NOT PERFORM ASH OF WAR ----- JUMPING");
                return false;
            }

            if (!playerPerformingAction.playerLocomotionManager.isGrounded)
            {
                Debug.Log("CAN NOT PERFORM ASH OF WAR --- - ---- NOT GROUNDED");
                return false;
            }

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            {
                Debug.Log("CAN NOT PERFORM ASH OF WAR - - --   OUT OF STAMINA");
                return false;
            }
            
            
            return true;
            

        }

        // SMALLER WEAPONS PERFORM FASTER PARRIES
        private void PerformParryTypeBasedOnWeapon(PlayerManager playerPerformingAction)
        {
            WeaponItem weaponBeingUsed = playerPerformingAction.playerCombatManager.currentWeaponBeingUsed;

            switch (weaponBeingUsed.weaponClass)
            {
                case WeaponClass.StraightSword:
                    break;
                case WeaponClass.Spear:
                    break;
                case WeaponClass.MediumShield:
                    playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Slow_Parry_01", true);
                    break;
                case WeaponClass.Fist:
                    break;
                case WeaponClass.LightShield:
                    playerPerformingAction.playerAnimatorManager.PlayTargetActionAnimation("Fast_Parry_01", true);
                    break;
            }


        }

    }
}