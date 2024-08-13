using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace AS
{
    [CreateAssetMenu(menuName = "A.I/States/Combat Stance")]
    public class CombatStanceState : AIState
    {

        //  select an attack for the attack state, depending on distance and angle of target in relation to the character
        //  process any combat logic here, whilst waiting to attack (blocking, strafing, dodging, etc. ...)
        //  if target moves out of combat range, return to the pursue target state
        //  if target is no lo0nger present (dead, disconnected, teleported etc...) switch to idle state

        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks;  //  A list of all possible attacks this character can do.
        protected List<AICharacterAttackAction> potentialAttacks;   //   all attacks possible in this situation (based on distance angle etc.)
        private AICharacterAttackAction choosenAttack;
        private AICharacterAttackAction previousAttack;
        protected bool hasAttack = false;

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false; //  if the char can do combo attack after the initial attack
        [SerializeField] protected int chanceToPerformCombo = 25; // chance (in percent) for performin the combo attack  
        protected bool hasRolledForComboChance = false;    // if we have already rolled for the chance during this state

        [Header("Engagement Distance")]
        [SerializeField] public float maximumEngagementDistance = 5; //  distance we have to be away from the target before we enter the pursue state

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isPerformingAction)
            {
                return this;
            }

            if (!aiCharacter.navMeshAgent.enabled)
            {
                aiCharacter.navMeshAgent.enabled = true;
            }

            //  IF YOU WANT THE AI CHAR TO FACE AND TURN TOWARDS ITS TARGET WHEN ITS OUTSIDE IT'S FOV INCLUDE THIS

            if (aiCharacter.aiCharacterCombatManager.enablePivot)
            {
                if (!aiCharacter.aiCharacterNetworkManager.isMoving.Value)
                {
                    if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                    {
                        aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                    }
                }
            }
            

            //  ROTATE TOWARDS AGENT (NAVMESH AGENT)
            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);


            //  IF OUR TARGET IS NO LONGER PRESENT, SWITCH TO IDLE
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }

            //  IF WE DO NOT HAVE AN ATTACK, GET ONE (LOOP)
            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                aiCharacter.attack.currentAttack = choosenAttack;
                //  ROLL FOR COMBO CHANCE
                return SwitchState(aiCharacter, aiCharacter.attack);
            }

            //  IF WE ARE OUTSIDE OF THE COMBAT ENGAGEMENT DISTANCE, SWITCH TO PURSUE TARGET STATE

            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;    

        }

        protected virtual void GetNewAttack(AICharacterManager aiCharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();

            foreach (var potentialAttack in aiCharacterAttacks)
            {
                //  IF WE ARE TOO CLOSE FOR THIS ATTACK, SKIP THIS, AND CHECK FOR THE NEXT ONE
                if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                {
                    continue;
                }
                //  IF WE ARE TOO FAR FOR THIS ATTACK, SKIP THIS, AND CHECK FOR THE NEXT ONE
                if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
                {
                    continue;
                }

                //  IF THE TARGET IS OUTSIDE MINIMUM FOV (FIELD OF VIEW) FOR THIS ATTACK, SKIP THIS ATTACK, CHECK THE NEXT
                if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                {
                    continue;
                }
                //  IF THE TARGET IS OUTSIDE MAXIMUM FOV (FIELD OF VIEW) FOR THIS ATTACK, SKIP THIS ATTACK, CHECK THE NEXT
                if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                {
                    continue;
                }

                potentialAttacks.Add(potentialAttack);
            }

            if (potentialAttacks.Count <= 0)
            {
                Debug.Log("COMBAT STANCE  >>>  GET NEW ATTACK >>> NO POTENTIAL ATTACK >>> Returned");
                return;
            }

            var totalWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }


            var randomWeightValue = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;

                if (randomWeightValue <= processedWeight)
                {
                    choosenAttack = attack;
                    previousAttack = choosenAttack;
                    hasAttack = true;
                    return;
                }
            }


            //  pick one of the remaining attacks randomly, based on weight
            //  Select this attack and pass it to the attack state





        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;

            int randomPercentage = Random.Range(0, 100);

            if (randomPercentage < outcomeChance)
            {
                outcomeWillBePerformed = true;
            }

            return outcomeWillBePerformed;
        }

        protected override void ResetStateFLags(AICharacterManager aiCharacter)
        {
            base.ResetStateFLags(aiCharacter);

            hasAttack = false;
            hasRolledForComboChance = false;
        }



    }

}
