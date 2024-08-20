using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{
    public class AIBossCharacterNetworkManager : AICharacterNetworkManager
    {
        AIBossCharacterManager aiBossCharacter;


        protected override void Awake()
        {
            base.Awake();

            aiBossCharacter = GetComponent<AIBossCharacterManager>();
        }


        public override void CheckHP(int oldValue, int newValue)
        {
            base.CheckHP(oldValue, newValue);

            if (aiBossCharacter.IsOwner)
            {
                if (currentHealth.Value <= 0)
                    return;
                float healthNeededForShift = maxHealth.Value * (aiBossCharacter.minimumHealthPercentageToShift / 100);
                if (currentHealth.Value <= healthNeededForShift)
                {
                    aiBossCharacter.PhaseShift();
                }
            }
            

        }




    }
}

