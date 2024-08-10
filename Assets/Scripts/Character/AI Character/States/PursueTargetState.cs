using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AS
{
    [CreateAssetMenu(menuName = "A.I/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {


            // CHECK IF WE ARE PERFORMING ACTION 
            if (aiCharacter.isPerformingAction)
            {
                return this;
            }

            // CHECK IF OUR TAGET IS NULL  IF WE DO NOT HAVE A TARGET RETURN TO IDLE STATE
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }

            // MAKE SURE OUR NAVMESH AGENT IS ACTIVE, IF ITS NOT ENABLE IT

            if (aiCharacter.navMeshAgent.enabled == false)
            {
                aiCharacter.navMeshAgent.enabled = true;
            }

            //  IF OUR TARGET GOES OUTSIDE OF THE CHARACTERS FOV PIVOT TO FACE THEM
            if (aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumFOV
                || aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumFOV)
            {
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }

            aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

            // IF WE ARE WITHIN COMBAT RANGE OF A TARGET, SWITCH STATE TO COMBAT STANCE STATE

            // IF THE TARGET IS NOT REACHABLE, AND THEY ARE FAR AWAY, RETURN HOME



            // PURSUE THE TARGET
            // OPTION 1:
            // aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position);

            // OPTION 2:
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }



    }
}

