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


        public override void AttemptRiposte(RaycastHit hit)
        {
            base.AttemptRiposte(hit);
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!This does something");
            CharacterManager targetCharacter = hit.transform.gameObject.GetComponent<CharacterManager>();

            // IF TARGET CHAR IS NULL RETURN
            if (targetCharacter == null)
                return;

            // IF TARGET CHAR IS NOT RIPOSTABLE SOMEHOW, RETUNR
            if (!targetCharacter.characterNetworkManager.isRipostable.Value)
                return;

            // IF TARGET IS ALREADY BEING CRITICALLY STRIKED BY SOMEONE ELSE, RETURN
            if (targetCharacter.characterNetworkManager.isBeingCriticallyDamaged.Value)
                return;

            MeleeWeaponItem riposteWeapon;
            MeleeWeaponDamageCollider riposteCollider;

            // TODO: CHECK IF WE ARE TWO HANDLING RIGHT OR LEFT WEAPON (THIS WILL CHANGE THE RIPOSTE WEAPON)

            riposteWeapon = player.playerInventoryManager.currentRightHandWeapon as MeleeWeaponItem;
            riposteCollider = player.playerEquipmentManager.rightWeaponManager.meleeDamageCollider;

            // THE RIPOSTE ANIMATOIN WILL CHANGE DEFENDNG ON THE WEAPON'S ANIMATO CONTROLLER
            character.characterAnimatorManager.PlayTargetActionAnimationInstantly("Riposte_01", true);

            // DURING CRITICAL STRIKE ANIMATION YOU ARE INVULNERABLE
            if (character.IsOwner)
                character.characterNetworkManager.isInvulnerable.Value = true;

            // makE a new damae effect for this damage type

            TakeCriticalDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeCriticalDamageEffect);

            // 2. APPLY ALL OF THE DAMAGE STATS FROM THE COLLIDER TO THE DANAGE EFFECT
            damageEffect.physicalDamage = riposteCollider.physicalDamage;
            damageEffect.magicDamage = riposteCollider.magicDamage;
            damageEffect.fireDamage = riposteCollider.fireDamage;
            damageEffect.lightningDamage = riposteCollider.lightningDamage;
            damageEffect.holyDamage = riposteCollider.holyDamage;
            damageEffect.poiseDamage = riposteCollider.poiseDamage;

            // 3. MULTIPLAY THE DAMAGE BY WEAPOS RIPOSTE MODIFIER
            damageEffect.physicalDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.magicDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.fireDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.lightningDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.holyDamage *= riposteWeapon.riposte_Attack_01_Modifier;
            damageEffect.poiseDamage *= riposteWeapon.riposte_Attack_01_Modifier;

            // USE A SERVER RPC SEND THE RIPOSTE TO THE TARGET, WHERE THEY WILL PLAY THE PROPER ANMATION ON THEIR END, AND TAKE DAMAGE
            targetCharacter.characterNetworkManager.NotifyTheServerOfRiposteServerRpc(
                targetCharacter.NetworkObjectId, character.NetworkObjectId, 
                "Riposted_01",
                riposteWeapon.itemID,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.lightningDamage,
                damageEffect.holyDamage,
                damageEffect.poiseDamage);
            Debug.Log("RIPOSTING TARGET: " + hit);
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
                case AttackType.LightAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.HeavyAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.heavyAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.ChargedAttack02:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.chargedAttackStaminaCostMultiplier;
                    break;
                case AttackType.RunningAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.runningAttackStaminaCostMultiplier;
                    break;
                case AttackType.RollingAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.rollingAttackStaminaCostMultiplier;
                    break;
                case AttackType.BackstepAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.backstepAttackStaminaCostMultiplier;
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
