using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    public class AIDurkCombatManager : AICharacterCombatManager
    {
        [Header("Damage Colliders")]
        [SerializeField] DurkClubDamageCollider clubDamageCollider;
        [SerializeField] Transform durksStompingFoot;
        [SerializeField] float stompAttackAOERadius = 1.5f;

        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.4f;
        [SerializeField] float attack03DamageModifier = 1.6f;
        [SerializeField] float stompDamage = 25;

        public void SetAttack01Damage()
        {
            clubDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            clubDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void SetAttack03Damage()
        {
            clubDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void OpenClubDamageCollider()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGrunt();
            clubDamageCollider.EnableDamageCollider();
        }

        public void CloseClubDamageCollider()
        {
            clubDamageCollider.DisableDamageCollider();
        }

        public void ActivateDurkStomp()
        {
            Collider[] colliders = Physics.OverlapSphere(durksStompingFoot.position, stompAttackAOERadius, WorldUtilityManager.instance.GetCharacterLayers());
            List<CharacterManager> charactersDamaged = new List<CharacterManager>();


            foreach (var collider in colliders)
            {
                CharacterManager character = collider.GetComponentInParent<CharacterManager>();

                if (character != null)
                {
                    if (charactersDamaged.Contains(character))
                    {
                        continue;
                    }

                    charactersDamaged.Add(character);

                    //  WE ONLY PROCESS DAMAGEIF THE CHARACTER "ISOWNER" SO THAT THEY ONLY GET DAMAGED IF THE COLLIDER CONNECTS ON
                    //  THEIR CLIENT, MEANING IF YOU ARE HIT ON THE HOSTS SCREEN BUT NOT ON YOUR OWN, YOU WILL NOT BE HIT.
                    if (character.IsOwner)
                    {
                        //  CHECK FOR BLOCK
                        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
                        damageEffect.physicalDamage = stompDamage;
                        damageEffect.poiseDamage = stompDamage;

                        character.characterEffectsManager.ProcessInstantEffect(damageEffect);
                    }
                }
                
            }

            

        }

        public override void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            //  PLAY A PIVOT ANIMATION  DEPENDING ON THE VIEWABLE ANGLE OF THE TARGET
            if (aiCharacter.isPerformingAction)
            {
                Debug.Log("ai char combat manager >>>      is performing action, returned");
                return;
            }

            
            if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_90", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_90", true);
            }
            
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Right_180", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn_Left_180", true);
            }
        }

    }
}

