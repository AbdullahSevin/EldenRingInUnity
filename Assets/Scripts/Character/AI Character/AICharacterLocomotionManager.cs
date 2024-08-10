using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {

        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }
        }


    }

}
