using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace AS
{
    public class CharacterCombatManager : NetworkBehaviour
    {
        protected CharacterManager character;

        [Header("Last Attack Animation Performed")]
        public string lastAttackAnimationPerformed;

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Lock On")]
        public Transform lockOnTransform;

        [Header("Attack Flags")]
        public bool canPerformRollingAttack = false;
        public bool canPerformBackstepAttack = false;


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (character.IsOwner)
            {
                if (newTarget != null)
                {
                    currentTarget = newTarget;
                    //  TELL THE NETWORK WE HAVE A TARGET, AND TELL THE NETWORK WHO IT IS
                    character.characterNetworkManager.currentTargetNetworkObjectID.Value = newTarget.GetComponent<NetworkObject>().NetworkObjectId;
                }
                else
                {
                    currentTarget = null;
                }
            }

        }

        public void EnableIsVulnerable()
        {
            if (character.IsOwner)
            {
                character.characterNetworkManager.isInvulnerable.Value = true;
            }

        }

        public void DisableIsVulnerable()
        {
            if (character.IsOwner)
            {
                character.characterNetworkManager.isInvulnerable.Value = false;
            }
        }

        public void EnableCanDoRollingAttack()
        {
            canPerformRollingAttack = true;  
        }
        public void DisableCanDoRollingAttack()
        {
            canPerformRollingAttack = false;
        }

        public void EnableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = true;
        }
        public void DisableCanDoBackstepAttack()
        {
            canPerformBackstepAttack = false;
        }



        public virtual void EnableCanDoCombo()
        {

        }


        public virtual void DisableCanDoCombo()
        {


        }

    }
}

