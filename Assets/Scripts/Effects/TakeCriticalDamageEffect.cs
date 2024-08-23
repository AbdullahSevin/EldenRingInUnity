using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    [CreateAssetMenu(menuName ="Character Effects/Instant Effects/Take Critical Damage Effect")]
    public class TakeCriticalDamageEffect : TakeDamageEffect
    {

        public override void ProcessEffect(CharacterManager character)
        {
            if (character.characterNetworkManager.isInvulnerable.Value)
            {
                return;
            }

            

            //  IF THE CHARACTER IS DEAD, NO ADDITIONAL DAMAGE EFFECTS SHOULD BE PROCESSED
            if (character.isDead.Value)
                return;


            CalculateDamage(character);

            character.characterCombatManager.pendingCriticalDamage = finalDamageDealt;  

            

        }

        protected override void CalculateDamage(CharacterManager character)
        {
            base.CalculateDamage(character);
            if (!character.IsOwner)
            {
                return;
            }

            if (characterCausingDamage != null)
            {
                // CHECK FOR DAMAGE MODIFIERS AND MODIFY BASE DAMAGE (Physical damage buff, elemental damage buff etc.)
            }

            //  CHECK CHARACTER FOR FLAT DEFENSES AND SUBSTRACT THEM FROM THE DAMAGE

            //  CHECK CHARACTER FOR ARMOR ABSORBTIONS, AND SUBSTRACT THE PERCENTAGE FROM THE DAMAGE

            //  AND ALL DAMAGE TYPES TOGETHER, AND APPLY FINAL DAMAGE

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            
            // Debug.Log("damage taken: " + finalDamageDealt);

            //  CALCULATE THE POISE DAMAGE TO DETERMINE IF THE CHARACTER WILL BE STUNNED

            // WE SUBSTRACT THE POISE DAMAGE FROM THE CHARACTERS TOTAL
            character.characterStatsManager.totalPoiseDamage -= poiseDamage;

            // WE STORE THE PREV POISE DMG TAKEN FOR OTHER INTERACTIONS
            character.characterCombatManager.previousPoiseDamageTaken = poiseDamage;

            float remainingPoise = character.characterStatsManager.basePoiseDefense +
                character.characterStatsManager.offensivePoiseBonus +
                character.characterStatsManager.totalPoiseDamage;

            if (remainingPoise <= 0)
            {
                poiseIsBroken = true;
            }

            //  SINCE THE CHARACTER HAS BEEN HIT, WE RESET THE POISE TIMER 
            character.characterStatsManager.poiseResetTimer = character.characterStatsManager.defaultPoiseResetTime ;
        }


    }
}

