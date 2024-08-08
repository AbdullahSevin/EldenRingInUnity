using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace AS
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        [Header("Flags")]
        public bool canComboWithMainHandWeapon = false;
        // public bool canComboWithOffHandWeapon = false;

        


        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (player.IsOwner)
            {
                //  PERFORM THE ACTION LOCALLY
                weaponAction.AttempToPerformAction(player, weaponPerformingAction);

                // notify the server we performed the action, so we perform it from their perspective also, (perform for other clients)
                player.playerNetworkManager.NotifyTheServerOfWeaponActionServerRpc(NetworkManager.Singleton.LocalClientId,
                    weaponAction.actionID, weaponPerformingAction.itemID);

            }



        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            if (!player.IsOwner)
            {
                return;
            }

            if (currentWeaponBeingUsed == null)
            {
                return;
            }
            
            

            float staminaDeducted = 0;

            switch (currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }
            //  Debug.Log("STAMINA DEDUCTED: " +  staminaDeducted);
            player.playerNetworkManager.currentStamina.Value -= Mathf.RoundToInt(staminaDeducted);

        }

        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);

            if (player.IsOwner)
            {
                PlayerCamera.instance.SetLockCameraHeight();
            }

        }

        

    }

}
