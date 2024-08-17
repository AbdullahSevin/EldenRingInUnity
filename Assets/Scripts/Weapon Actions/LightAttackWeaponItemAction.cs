using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [Header("Light Attacks")]
        [SerializeField] string light_Attack_01 = "Main_Light_Attack_01";  // Main = main hand
        [SerializeField] string light_Attack_02 = "Main_Light_Attack_02";

        [Header("Running Attacks")]
        [SerializeField] string running_Attack_01 = "Running_Attack_01";
        [Header("Rolling Attacks")]
        [SerializeField] string rolling_Attack_01 = "Rolling_Attack_01";
        [Header("Backstep Attacks")]
        [SerializeField] string backstep_Attack_01 = "Backstep_Attack_01";


        public override void AttempToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {

            base.AttempToPerformAction(playerPerformingAction, weaponPerformingAction);

            if (!playerPerformingAction.IsOwner)
            {
                return;
            }

            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
            {
                return;
            }

            if (!playerPerformingAction.characterLocomotionManager.isGrounded)
            {
                return;
            }

            if (playerPerformingAction.IsOwner)
            {
                playerPerformingAction.playerNetworkManager.isAttacking.Value = true;
            }

            //  IF WE ARE SPRINTING, PERFORM RUNNING ATTACK 
            if (playerPerformingAction.characterNetworkManager.isSprinting.Value)
            {
                PerformRunningAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }

            //  IF WE ARE ROLLING, PERFORM ROLLING ATTACK
            if (playerPerformingAction.characterCombatManager.canPerformRollingAttack)
            {
                PerformRollingAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }

            //  IF WE ARE backstepping, PERFORM backstep ATTACK
            if (playerPerformingAction.characterCombatManager.canPerformBackstepAttack)
            {
                PerformBackstepAttack(playerPerformingAction, weaponPerformingAction);
                return;
            }


            PerformLightAttack(playerPerformingAction, weaponPerformingAction);

        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // IF WE ARE ATTACKING CURRENTLY, AND WE ARE ABLE TO PERFORM A COMBO, PERFORM THE COMBO ATTACK
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;

                //  PERFORM A ATTACK BASED ON THE PREVIOUS ATTACK WE JUST PLAYED
                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == light_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack02, light_Attack_02, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
                }
            }

            //  OTHERWISE, IF WE ARE NOT ALREADY ATTACKING JUST PERFORM A REGULAR ATTACK
            else if (!playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);

            }

        }

        private void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            //  IF WE ARE 2 HANDING PLAY 2 HANDING RUNNING ATTACK ANIM (TODO)
            //  ELSE PERFORM A ONE HAND RUN ATTACK ANIM

            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RunningAttack01, running_Attack_01, true, applyRootMotion :true, canRotate : true, canMove: true);


        }

        private void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            //  IF WE ARE 2 HANDING PLAY 2 HANDING rolling ATTACK ANIM (TODO)
            //  ELSE PERFORM A ONE HAND roll ATTACK ANIM
            playerPerformingAction.playerCombatManager.canPerformRollingAttack = false;
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.RollingAttack01, rolling_Attack_01, true);


        }

        private void PerformBackstepAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            //  IF WE ARE 2 HANDING PLAY 2 HANDING rolling ATTACK ANIM (TODO)
            //  ELSE PERFORM A ONE HAND roll ATTACK ANIM
            playerPerformingAction.playerCombatManager.canPerformBackstepAttack = false;
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(weaponPerformingAction, AttackType.BackstepAttack01, backstep_Attack_01, true);


        }

    }
}

