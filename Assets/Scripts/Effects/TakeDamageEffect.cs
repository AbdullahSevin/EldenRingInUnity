using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AS
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {

        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; // If the damage is caused by another character's attack it will be stored here

        [Header("Damage")]
        public float physicalDamage = 0;  // (In the future will be split into "Standart", "Strike", "Slahs", and "Pierce")
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;


        [Header("Final Damage")]
        private int finalDamageDealt = 0;                 //  The damage the character takes after ALL calculations have been made

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;                 //  If a character's Poise is Broken, They will be "Stunned" and play a damage animation

        // TO DO > BUILD UPS 
        // build up effect amounts

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;          //  USED ON TOP OF REGULAR SFX IF THERE IS ELEMENTAL DAMAGE PRESENT (Magic/Fire/Lightning/Holy)

        [Header("Direction Damage Taken From")] 
        public float angleHitFrom;                        //  USED TO DETERMINE WHAT DAMAGE ANIMATION TO PLAY (Move backwards, to the left, to the right etc) (i.e. Light_hit_taken_left_01.anim)
        public Vector3 contactPoint;                      //  USED TO DETERMINE WHERE THE BLOOD FX INSTANTIATE



        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            //  IF THE CHARACTER IS DEAD, NO ADDITIONAL DAMAGE EFFECTS SHOULD BE PROCESSED
            if (character.isDead.Value)
                return;

            //  CHECK FOR INVULNERABILITY

            CalculateDamage(character);
            //  CHECK WHICH DIRECTION THE DAMAGE CAME FROM
            //  PLAY A DAMAGE ANIMATION
            //  CHECK FOR BUILD UPS (POISON, BLEED ETC...)
            //  PLAY DAMAGE SOUND FX
            //  PLAY DAMAGE VFX (BLOOD)

            //  IF CHARACTER IS A.I., CHECK FOR NEW TARGET IF CHARACTER CAUSING DAMAGE IS PRESENT

        }

        private void CalculateDamage(CharacterManager character)
        {

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

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
            Debug.Log("damage taken: " + finalDamageDealt);
            
            //  CALCULATE THE POISE DAMAGE TO DETERMINE IF THE CHARACTER WILL BE STUNNED


        }

    }

}