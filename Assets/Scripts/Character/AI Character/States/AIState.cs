using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aICharacter)
        {
            return this;
        }

        protected virtual AIState SwitchState(AICharacterManager aiCharacter, AIState newState)
        {
            ResetStateFLags(aiCharacter);
            return newState;
        }

        protected virtual void ResetStateFLags(AICharacterManager aiCharacter)
        {
            // RESET ANY STATE FLAGS HERE SO WHEN YOU RETURN TO THE STATE, THEY ARE BLAN ONCE AGAIN
        }

    }
}

