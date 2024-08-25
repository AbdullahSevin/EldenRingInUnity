using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage; // (When calculating damage this will be used to check for attackers damage modifiers etc.)

        [Header("Weapon Attack Modifiers")]
        public float light_Attack_01_Modifier;
        public float light_Attack_02_Modifier;
        public float heavy_Attack_01_Modifier;
        public float heavy_Attack_02_Modifier;
        public float charge_Attack_01_Modifier;
        public float charge_Attack_02_Modifier;
        public float running_Attack_01_Modifier;
        public float rolling_Attack_01_Modifier;
        public float backstep_Attack_01_Modifier;

        protected override void Awake()
        {
            base.Awake();

            if (damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }
            damageCollider.enabled= false; //  MELEE WEAPON COLLIDERS SHOULD BE DISABLED  AT START, ONLY ENABLED WHEN ANIMATIONS ALLOW

        }

        protected override void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            


            if (damageTarget != null)
            {
                
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                if (damageTarget == characterCausingDamage)
                {
                    
                    return;
                }
                    

                //  CHECK IF WE CAN DAMAGE THIS TARGET BASED ON FRIENDLY FIRE
                if (!WorldUtilityManager.instance.CanIDamageThisTarget(characterCausingDamage.characterGroup, damageTarget.characterGroup))
                {
                    
                    return;
                }
                    

                //  CHECK IF TARGET IS PARRYING
                CheckForParry(damageTarget);

                //  CHECK IF TARGET IS BLOCKING
                CheckForBlock(damageTarget);

                //  CHECK IF TARGET IS INVULNERABLE
                if (!damageTarget.characterNetworkManager.isInvulnerable.Value)
                {
                    
                    DamageTarget(damageTarget);
                }
                         //  DAMAGE

            }          

        }

        protected override void CheckForParry(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
                return;

            if (!characterCausingDamage.characterNetworkManager.isParryable.Value)
                return;

            if (!damageTarget.IsOwner)
                return;

            if (damageTarget.characterNetworkManager.isParrying.Value)
            {
                charactersDamaged.Add(damageTarget);
                damageTarget.characterNetworkManager.NotifyServerOfParryServerRpc(characterCausingDamage.NetworkObjectId);
                //damageTarget.characterAnimatorManager.PlayTargetActionAnimationInstantly("Parry_Land_01", true);
            }


        }

        protected override void GetBlockingDotValues(CharacterManager damageTarget)
        {
            directionFromAttackToDamageTarget = characterCausingDamage.transform.position - damageTarget.transform.position;
            dotValueFromAttackToDamageTarget = Vector3.Dot(directionFromAttackToDamageTarget, damageTarget.transform.forward);
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
            {
                return;
            }

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.contactPoint = contactPoint;
            damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.LightAttack02:
                    ApplyAttackDamageModifiers(light_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack01:
                    ApplyAttackDamageModifiers(heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack02:
                    ApplyAttackDamageModifiers(heavy_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack01:
                    ApplyAttackDamageModifiers(charge_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack02:
                    ApplyAttackDamageModifiers(charge_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.RunningAttack01:
                    ApplyAttackDamageModifiers(running_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.RollingAttack01:
                    ApplyAttackDamageModifiers(rolling_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.BackstepAttack01:
                    ApplyAttackDamageModifiers(backstep_Attack_01_Modifier, damageEffect);
                    break;
                default:
                    break;

            }

            //damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

            if (characterCausingDamage.IsOwner)
            {
                damageTarget.characterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(
                    damageTarget.NetworkObjectId,
                    characterCausingDamage.NetworkObjectId,
                    damageEffect.physicalDamage,
                    damageEffect.magicDamage,
                    damageEffect.fireDamage,
                    damageEffect.lightningDamage,
                    damageEffect.holyDamage,
                    damageEffect.poiseDamage,
                    damageEffect.angleHitFrom,
                    damageEffect.contactPoint.x,
                    damageEffect.contactPoint.y,
                    damageEffect.contactPoint.z);
            }
        }

        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifier;
            damage.magicDamage *= modifier;
            damage.fireDamage *= modifier;
            damage.lightningDamage *= modifier;
            damage.holyDamage *= modifier;
            damage.poiseDamage *= modifier;

            //  IF ATTACK IS A FULLY CHARGED HEAVY, MULTIPLY BY FULL CHARGE MODIFIER AFTER NORMAL MODIFIER HAVE BEEN CALCULATED
        }


    }
}

