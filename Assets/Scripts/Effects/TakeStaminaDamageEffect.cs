using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AS
{

    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
    public class TakeStaminaDamageEffect : InstantCharacterEffect
    {

        public float staminaDamage;
        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterManager character)
        {
            if (character.IsOwner)
            {
                Debug.Log("CAHARACTER TOOK STAMINA DAMAGE " + staminaDamage);
                character.characterNetworkManager.currentStamina.Value -= staminaDamage;
            }
        }



    }

}

