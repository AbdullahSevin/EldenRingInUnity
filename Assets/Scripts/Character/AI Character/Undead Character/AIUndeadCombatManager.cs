using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class AIUndeadCombatManager : AICharacterCombatManager
    {

        [Header("Damage Colliders")]
        [SerializeField] UndeadHandDamageCollider rightHandDamageCollider;
        [SerializeField] UndeadHandDamageCollider leftHandDamageCollider;


        [Header("Damage")]
        [SerializeField] int baseDamage = 25;
        [SerializeField] float attack01DamageModifier = 1.0f;
        [SerializeField] float attack02DamageModifier = 1.4f;

        public void SetAttack01Damage()
        {
            rightHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
            leftHandDamageCollider.physicalDamage = baseDamage * attack01DamageModifier;
        }

        public void SetAttack02Damage()
        {
            rightHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
            leftHandDamageCollider.physicalDamage = baseDamage * attack02DamageModifier;
        }

        public void OpenRightHandDamageCollider()
        {
            aiCharacter.characterSoundFXManager.PlayAttackGrunt();
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseRightHandDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void OpenLeftHandDamageCollider()
        {
            // aiCharacter.characterSoundFXManager.PlayAttackGrunt();
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void CloseLeftHandDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }











    }
}

