using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    [CreateAssetMenu(menuName = "A.I/States/Attack")]
    public class AttackState : AIState
    {
        [Header("Current Attack")]
        [HideInInspector] public AICharacterAttackAction currentAttack;
        [HideInInspector] public bool willPerformCombo = false;

        [Header("State Flags")]
        protected bool hasPerformedAttack = false;
        protected bool hasPerformedCombo = false;


        [Header("Pivot After Attack")]
        [SerializeField] protected bool pivotAfterAttack = false;

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }

            if (aiCharacter.aiCharacterCombatManager.currentTarget.isDead.Value)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }

            aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhilstAttacking(aiCharacter);

            aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

            //  PERFORM A COMBO
            if (willPerformCombo && !hasPerformedCombo)
            {
                if (currentAttack.comboAction != null)
                {
                    //  IF CAN COMBO
                    //  hasPerformedCombo = true;
                    //  currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
                }
            }

            if (aiCharacter.isPerformingAction)
            {
                return this;
            }

            if (!hasPerformedAttack)
            {
                //  IF WE ARE STILL RECOVERING FROM AN ACTION, WAIT BEFORE PERFORMING ANOTHER
                if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
                {
                    return this;
                }

                PerformAttack(aiCharacter);

                //  RETURN TO THE TOP, SO IF WE HAVE A COMBO WO PROCESS THAT WHEN WE ARE ABLE
                return this;
            }

            if (pivotAfterAttack)
            {
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }

            return SwitchState(aiCharacter, aiCharacter.combatStance);
        }

        protected void PerformAttack(AICharacterManager aiCharacter)
        {
            hasPerformedAttack = true;
            currentAttack.AttemptToPerformAction(aiCharacter);
            aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;
        }

        protected override void ResetStateFLags(AICharacterManager aiCharacter)
        {
            base.ResetStateFLags(aiCharacter);

            hasPerformedAttack = false;
            hasPerformedCombo = false;
        }

    }
}

